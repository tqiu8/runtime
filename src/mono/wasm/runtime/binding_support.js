// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

var BindingSupportLib = {
	$BINDING__postset: 'BINDING.export_functions (Module);',
	$BINDING: {
		BINDING_ASM: "[System.Private.Runtime.InteropServices.JavaScript]System.Runtime.InteropServices.JavaScript.Runtime",
		mono_wasm_object_registry: [],
		mono_wasm_ref_counter: 0,
		mono_wasm_free_list: [],
		mono_wasm_owned_objects_frames: [],
		mono_wasm_owned_objects_LMF: [],
		mono_wasm_marshal_enum_as_int: false,
		mono_bindings_init: function (binding_asm) {
			this.BINDING_ASM = binding_asm;
		},

		export_functions: function (module) {
			module ["mono_bindings_init"] = BINDING.mono_bindings_init.bind(BINDING);
			module ["mono_bind_method"] = BINDING.bind_method.bind(BINDING);
			module ["mono_method_invoke"] = BINDING.call_method.bind(BINDING);
			module ["mono_method_get_call_signature"] = BINDING.mono_method_get_call_signature.bind(BINDING);
			module ["mono_method_resolve"] = BINDING.resolve_method_fqn.bind(BINDING);
			module ["mono_bind_static_method"] = BINDING.bind_static_method.bind(BINDING);
			module ["mono_call_static_method"] = BINDING.call_static_method.bind(BINDING);
			module ["mono_bind_assembly_entry_point"] = BINDING.bind_assembly_entry_point.bind(BINDING);
			module ["mono_call_assembly_entry_point"] = BINDING.call_assembly_entry_point.bind(BINDING);
		},

		bindings_lazy_init: function () {
			if (this.init)
				return;

			// avoid infinite recursion
			this.init = true;
		
			Array.prototype[Symbol.for("wasm type")] = 1;
			ArrayBuffer.prototype[Symbol.for("wasm type")] = 2;
			DataView.prototype[Symbol.for("wasm type")] = 3;
			Function.prototype[Symbol.for("wasm type")] =  4;
			Map.prototype[Symbol.for("wasm type")] = 5;
			if (typeof SharedArrayBuffer !== "undefined")
				SharedArrayBuffer.prototype[Symbol.for("wasm type")] =  6;
			Int8Array.prototype[Symbol.for("wasm type")] = 10;
			Uint8Array.prototype[Symbol.for("wasm type")] = 11;
			Uint8ClampedArray.prototype[Symbol.for("wasm type")] = 12;
			Int16Array.prototype[Symbol.for("wasm type")] = 13;
			Uint16Array.prototype[Symbol.for("wasm type")] = 14;
			Int32Array.prototype[Symbol.for("wasm type")] = 15;
			Uint32Array.prototype[Symbol.for("wasm type")] = 16;
			Float32Array.prototype[Symbol.for("wasm type")] = 17;
			Float64Array.prototype[Symbol.for("wasm type")] = 18;

			this.assembly_load = Module.cwrap ('mono_wasm_assembly_load', 'number', ['string']);
			this.find_class = Module.cwrap ('mono_wasm_assembly_find_class', 'number', ['number', 'string', 'string']);
			this._find_method = Module.cwrap ('mono_wasm_assembly_find_method', 'number', ['number', 'string', 'number']);
			this.invoke_method = Module.cwrap ('mono_wasm_invoke_method', 'number', ['number', 'number', 'number', 'number']);
			this.mono_string_get_utf8 = Module.cwrap ('mono_wasm_string_get_utf8', 'number', ['number']);
			this.mono_wasm_string_from_utf16 = Module.cwrap ('mono_wasm_string_from_utf16', 'number', ['number', 'number']);
			this.mono_get_obj_type = Module.cwrap ('mono_wasm_get_obj_type', 'number', ['number']);
			this.mono_unbox_int = Module.cwrap ('mono_unbox_int', 'number', ['number']);
			this.mono_unbox_float = Module.cwrap ('mono_wasm_unbox_float', 'number', ['number']);
			this.mono_array_length = Module.cwrap ('mono_wasm_array_length', 'number', ['number']);
			this.mono_array_get = Module.cwrap ('mono_wasm_array_get', 'number', ['number', 'number']);
			this.mono_obj_array_new = Module.cwrap ('mono_wasm_obj_array_new', 'number', ['number']);
			this.mono_obj_array_set = Module.cwrap ('mono_wasm_obj_array_set', 'void', ['number', 'number', 'number']);
			this.mono_wasm_register_bundled_satellite_assemblies = Module.cwrap ('mono_wasm_register_bundled_satellite_assemblies', 'void', [ ]);
			this.mono_unbox_enum = Module.cwrap ('mono_wasm_unbox_enum', 'number', ['number']);
			this.assembly_get_entry_point = Module.cwrap ('mono_wasm_assembly_get_entry_point', 'number', ['number']);

			// receives a byteoffset into allocated Heap with a size.
			this.mono_typed_array_new = Module.cwrap ('mono_wasm_typed_array_new', 'number', ['number','number','number','number']);

			var binding_fqn_asm = this.BINDING_ASM.substring(this.BINDING_ASM.indexOf ("[") + 1, this.BINDING_ASM.indexOf ("]")).trim();
			var binding_fqn_class = this.BINDING_ASM.substring (this.BINDING_ASM.indexOf ("]") + 1).trim();
			
			this.binding_module = this.assembly_load (binding_fqn_asm);
			if (!this.binding_module)
				throw "Can't find bindings module assembly: " + binding_fqn_asm;

			var namespace = null, classname = null;
			if (binding_fqn_class !== null && typeof binding_fqn_class !== "undefined")
			{
				namespace = "System.Runtime.InteropServices.JavaScript";
				classname = binding_fqn_class.length > 0 ? binding_fqn_class : "Runtime";
				if (binding_fqn_class.indexOf(".") != -1) {
					var idx = binding_fqn_class.lastIndexOf(".");
					namespace = binding_fqn_class.substring (0, idx);
					classname = binding_fqn_class.substring (idx + 1);
				}
			}

			var wasm_runtime_class = this.find_class (this.binding_module, namespace, classname);
			if (!wasm_runtime_class)
				throw "Can't find " + binding_fqn_class + " class";

			var get_method = function(method_name) {
				var res = BINDING.find_method (wasm_runtime_class, method_name, -1);
				if (!res)
					throw "Can't find method " + namespace + "." + classname + ":" + method_name;
				return res;
			};

			var bind_runtime_method = function (method_name, signature) {
				var method = get_method (method_name);
				return BINDING.bind_method (method, 0, signature, "BINDINGS_" + method_name);
			};

			this.bind_js_obj = get_method ("BindJSObject");
			this.bind_core_clr_obj = get_method ("BindCoreCLRObject");
			this.bind_existing_obj = get_method ("BindExistingObject");
			this.unbind_raw_obj_and_free = get_method ("UnBindRawJSObjectAndFree");			
			this.get_js_id = get_method ("GetJSObjectId");
			this.get_raw_mono_obj = get_method ("GetDotNetObject");

			this.box_js_int = get_method ("BoxInt");
			this.box_js_double = get_method ("BoxDouble");
			this.box_js_bool = get_method ("BoxBool");
			this._is_simple_array = bind_runtime_method ("IsSimpleArray", "m");
			this.setup_js_cont = get_method ("SetupJSContinuation");

			this.create_tcs = get_method ("CreateTaskSource");
			this.set_tcs_result = get_method ("SetTaskSourceResult");
			this.set_tcs_failure = get_method ("SetTaskSourceFailure");
			this.tcs_get_task_and_bind = get_method ("GetTaskAndBind");
			this.get_call_sig = get_method ("GetCallSignature");

			this.object_to_string = get_method ("ObjectToString");
			this.get_date_value = get_method ("GetDateValue");
			this.create_date_time = get_method ("CreateDateTime");
			this.create_uri = get_method ("CreateUri");

			this.safehandle_addref = get_method ("SafeHandleAddRef");
			this.safehandle_release = get_method ("SafeHandleRelease");
			this.safehandle_get_handle = get_method ("SafeHandleGetHandle");
			this.safehandle_release_by_handle = get_method ("SafeHandleReleaseByHandle");
		},

		find_method: function (klass, name, n) {
			var result = this._find_method(klass, name, n);
			if (!this._method_descriptions)
				this._method_descriptions = new Map();
			this._method_descriptions.set(result, name);
			return result;
		},

		get_js_obj: function (js_handle) {
			if (js_handle > 0)
				return this.mono_wasm_require_handle(js_handle);
			return null;
		},

		conv_string: function (mono_obj) {
			return MONO.string_decoder.copy (mono_obj);
		},

		is_nested_array: function (ele) {
			return this._is_simple_array(ele);
		},

		js_string_to_mono_string: function (string) {
			if (string === null || typeof string === "undefined")
				return 0;

			var buffer = Module._malloc ((string.length + 1) * 2);
			var buffer16 = (buffer / 2) | 0;
			for (var i = 0; i < string.length; i++)
				Module.HEAP16[buffer16 + i] = string.charCodeAt (i);
			Module.HEAP16[buffer16 + string.length] = 0;
			var result = this.mono_wasm_string_from_utf16 (buffer, string.length);
			Module._free (buffer);
			return result;
		},
		
		mono_array_to_js_array: function (mono_array) {
			if (mono_array === 0)
				return null;

			var arrayRoot = MONO.mono_wasm_new_root (mono_array);
			try {
				return this._mono_array_to_js_array_rooted (arrayRoot);
			} finally {
				arrayRoot.release();
			}
		},

		_mono_array_to_js_array_rooted: function (arrayRoot) {
			if (arrayRoot.value === 0)
				return null;
			
			let elemRoot = MONO.mono_wasm_new_root (); 

			try {
				var len = this.mono_array_length (arrayRoot.value);
				var res = new Array (len);
				for (var i = 0; i < len; ++i)
				{
					elemRoot.value = this.mono_array_get (arrayRoot.value, i);
					
					if (this.is_nested_array (elemRoot.value))
						res[i] = this._mono_array_to_js_array_rooted (elemRoot);
					else
						res[i] = this._unbox_mono_obj_rooted (elemRoot);
				}
			} finally {
				elemRoot.release ();
			}

			return res;
		},

		js_array_to_mono_array: function (js_array) {
			var mono_array = this.mono_obj_array_new (js_array.length);
			let [arrayRoot, elemRoot] = MONO.mono_wasm_new_roots ([mono_array, 0]);
			
			try {
				for (var i = 0; i < js_array.length; ++i) {
					elemRoot.value = this.js_to_mono_obj (js_array [i]);
					this.mono_obj_array_set (arrayRoot.value, i, elemRoot.value);
				}

				return mono_array;
			} finally {
				MONO.mono_wasm_release_roots (arrayRoot, elemRoot);
			}
		},

		unbox_mono_obj: function (mono_obj) {
			if (mono_obj === 0)
				return undefined;

			var root = MONO.mono_wasm_new_root (mono_obj);
			try {
				return this._unbox_mono_obj_rooted (root);
			} finally {
				root.release();
			}
		},

		_unbox_mono_obj_rooted: function (root) {
			var mono_obj = root.value;
			if (mono_obj === 0)
				return undefined;
			
			var type = this.mono_get_obj_type (mono_obj);
			//See MARSHAL_TYPE_ defines in driver.c
			switch (type) {
			case 1: // int
				return this.mono_unbox_int (mono_obj);
			case 2: // float
				return this.mono_unbox_float (mono_obj);
			case 3: //string
				return this.conv_string (mono_obj);
			case 4: //vts
				throw new Error ("no idea on how to unbox value types");
			case 5: { // delegate
				var obj = this.extract_js_obj (mono_obj);
				obj.__mono_delegate_alive__ = true;
				// FIXME: Should we root the object as long as this function has not been GCd?
				return function () {
					return BINDING.invoke_delegate (obj, arguments);
				};
			}
			case 6: {// Task

				if (typeof Promise === "undefined" || typeof Promise.resolve === "undefined")
					throw new Error ("Promises are not supported thus C# Tasks can not work in this context.");

				var obj = this.extract_js_obj (mono_obj);
				var cont_obj = null;
				var promise = new Promise (function (resolve, reject) {
					cont_obj = {
						resolve: resolve,
						reject: reject
					};
				});

				this.call_method (this.setup_js_cont, null, "mo", [ mono_obj, cont_obj ]);
				obj.__mono_js_cont__ = cont_obj.__mono_gchandle__;
				cont_obj.__mono_js_task__ = obj.__mono_gchandle__;
				return promise;
			}

			case 7: // ref type
				return this.extract_js_obj (mono_obj);

			case 8: // bool
				return this.mono_unbox_int (mono_obj) != 0;

			case 9: // enum

				if(this.mono_wasm_marshal_enum_as_int)
				{
					return this.mono_unbox_enum (mono_obj);
				}
				else
				{
					enumValue = this.call_method(this.object_to_string, null, "m", [ mono_obj ]);
				}

				return enumValue;

			case 10: // arrays
			case 11: 
			case 12: 
			case 13: 
			case 14: 
			case 15: 
			case 16: 
			case 17: 
			case 18:
			{
				throw new Error ("Marshalling of primitive arrays are not supported.  Use the corresponding TypedArray instead.");
			}
			case 20: // clr .NET DateTime
				var dateValue = this.call_method(this.get_date_value, null, "md", [ mono_obj ]);
				return new Date(dateValue);
			case 21: // clr .NET DateTimeOffset
				var dateoffsetValue = this.call_method(this.object_to_string, null, "m", [ mono_obj ]);
				return dateoffsetValue;
			case 22: // clr .NET Uri
				var uriValue = this.call_method(this.object_to_string, null, "m", [ mono_obj ]);
				return uriValue;
			case 23: // clr .NET SafeHandle
				var addRef = true;
				var js_handle = this.call_method(this.safehandle_get_handle, null, "mi", [ mono_obj, addRef ]);
				// FIXME: Is this a GC object that needs to be rooted?
				var requiredObject = BINDING.mono_wasm_require_handle (js_handle);
				if (addRef)
				{
					if (typeof this.mono_wasm_owned_objects_LMF === "undefined")
						this.mono_wasm_owned_objects_LMF = [];

					this.mono_wasm_owned_objects_LMF.push(js_handle);
				}
				return requiredObject;
			default:
				throw new Error ("no idea on how to unbox object kind " + type + " at offset " + mono_obj);
			}
		},

		create_task_completion_source: function () {
			return this.call_method (this.create_tcs, null, "i", [ -1 ]);
		},

		set_task_result: function (tcs, result) {
			tcs.is_mono_tcs_result_set = true;
			this.call_method (this.set_tcs_result, null, "oo", [ tcs, result ]);
			if (tcs.is_mono_tcs_task_bound)
				this.free_task_completion_source(tcs);
		},

		set_task_failure: function (tcs, reason) {
			tcs.is_mono_tcs_result_set = true;
			this.call_method (this.set_tcs_failure, null, "os", [ tcs, reason.toString () ]);
			if (tcs.is_mono_tcs_task_bound)
				this.free_task_completion_source(tcs);
		},

		// https://github.com/Planeshifter/emscripten-examples/blob/master/01_PassingArrays/sum_post.js
		js_typedarray_to_heap: function(typedArray){
			var numBytes = typedArray.length * typedArray.BYTES_PER_ELEMENT;
			var ptr = Module._malloc(numBytes);
			var heapBytes = new Uint8Array(Module.HEAPU8.buffer, ptr, numBytes);
			heapBytes.set(new Uint8Array(typedArray.buffer, typedArray.byteOffset, numBytes));
			return heapBytes;
		},
		js_to_mono_obj: function (js_obj) {
			this.bindings_lazy_init ();

			// determines if the javascript object is a Promise or Promise like which can happen
			// when using an external Promise library.  The javascript object should be marshalled 
			// as managed Task objects.
			// 
			// Example is when Bluebird is included in a web page using a script tag, it overwrites the 
			// global Promise object by default with its own version of Promise.
			function isThenable() {
				// When using an external Promise library the Promise.resolve may not be sufficient
				// to identify the object as a Promise.
				return Promise.resolve(js_obj) === js_obj || 
						((typeof js_obj === "object" || typeof js_obj === "function") && typeof js_obj.then === "function")
			}

			switch (true) {
				case js_obj === null:
				case typeof js_obj === "undefined":
					return 0;
				case typeof js_obj === "number": {
					if (parseInt(js_obj) == js_obj)
						result = this.call_method (this.box_js_int, null, "i!", [ js_obj ]);
					else
						result = this.call_method (this.box_js_double, null, "d!", [ js_obj ]);

					/*
						var unboxed = this.unbox_mono_obj (result);
						if (unboxed != js_obj)
							console.warn ("box->unbox cycle failed", js_obj, unboxed);
					*/
					
					return result;
				} case typeof js_obj === "string":
					return this.js_string_to_mono_string (js_obj);
				case typeof js_obj === "boolean":
					return this.call_method (this.box_js_bool, null, "i!", [ js_obj ]);
				case isThenable() === true:
					var the_task = this.try_extract_mono_obj (js_obj);
					if (the_task)
						return the_task;
					// FIXME: We need to root tcs for an appropriate timespan
					var tcs = this.create_task_completion_source ();
					js_obj.then (function (result) {
						BINDING.set_task_result (tcs, result);
					}, function (reason) {
						BINDING.set_task_failure (tcs, reason);
					})
					return this.get_task_and_bind (tcs, js_obj);
				case js_obj.constructor.name === "Date":
					// We may need to take into account the TimeZone Offset
					return this.call_method(this.create_date_time, null, "d!", [ js_obj.getTime() ]);
				default:
					return this.extract_mono_obj (js_obj);
			}
		},
		js_to_mono_uri: function (js_obj) {
			this.bindings_lazy_init ();

			switch (true) {
				case js_obj === null:
				case typeof js_obj === "undefined":
					return 0;
				case typeof js_obj === "string":
					return this.call_method(this.create_uri, null, "s!", [ js_obj ])
				default:
					return this.extract_mono_obj (js_obj);
			}
		},

		js_typed_array_to_array : function (js_obj) {

			// JavaScript typed arrays are array-like objects and provide a mechanism for accessing 
			// raw binary data. (...) To achieve maximum flexibility and efficiency, JavaScript typed arrays 
			// split the implementation into buffers and views. A buffer (implemented by the ArrayBuffer object)
			//  is an object representing a chunk of data; it has no format to speak of, and offers no 
			// mechanism for accessing its contents. In order to access the memory contained in a buffer, 
			// you need to use a view. A view provides a context — that is, a data type, starting offset, 
			// and number of elements — that turns the data into an actual typed array.
			// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Typed_arrays
			if (!!(js_obj.buffer instanceof ArrayBuffer && js_obj.BYTES_PER_ELEMENT)) 
			{
				var arrayType = js_obj[Symbol.for("wasm type")];
				var heapBytes = this.js_typedarray_to_heap(js_obj);
				var bufferArray = this.mono_typed_array_new(heapBytes.byteOffset, js_obj.length, js_obj.BYTES_PER_ELEMENT, arrayType);
				Module._free(heapBytes.byteOffset);
				return bufferArray;
			}
			else {
				throw new Error("Object '" + js_obj + "' is not a typed array");
			} 


		},
		// Copy the existing typed array to the heap pointed to by the pinned array address
		// 	 typed array memory -> copy to heap -> address of managed pinned array
		typedarray_copy_to : function (typed_array, pinned_array, begin, end, bytes_per_element) {

			// JavaScript typed arrays are array-like objects and provide a mechanism for accessing 
			// raw binary data. (...) To achieve maximum flexibility and efficiency, JavaScript typed arrays 
			// split the implementation into buffers and views. A buffer (implemented by the ArrayBuffer object)
			//  is an object representing a chunk of data; it has no format to speak of, and offers no 
			// mechanism for accessing its contents. In order to access the memory contained in a buffer, 
			// you need to use a view. A view provides a context — that is, a data type, starting offset, 
			// and number of elements — that turns the data into an actual typed array.
			// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Typed_arrays
			if (!!(typed_array.buffer instanceof ArrayBuffer && typed_array.BYTES_PER_ELEMENT)) 
			{
				// Some sanity checks of what is being asked of us
				// lets play it safe and throw an error here instead of assuming to much.
				// Better safe than sorry later
				if (bytes_per_element !== typed_array.BYTES_PER_ELEMENT)
					throw new Error("Inconsistent element sizes: TypedArray.BYTES_PER_ELEMENT '" + typed_array.BYTES_PER_ELEMENT + "' sizeof managed element: '" + bytes_per_element + "'");

				// how much space we have to work with
				var num_of_bytes = (end - begin) * bytes_per_element;
				// how much typed buffer space are we talking about
				var view_bytes = typed_array.length * typed_array.BYTES_PER_ELEMENT;
				// only use what is needed.
				if (num_of_bytes > view_bytes)
					num_of_bytes = view_bytes;

				// offset index into the view
				var offset = begin * bytes_per_element;

				// Create a view over the heap pointed to by the pinned array address
				var heapBytes = new Uint8Array(Module.HEAPU8.buffer, pinned_array + offset, num_of_bytes);
				// Copy the bytes of the typed array to the heap.
				heapBytes.set(new Uint8Array(typed_array.buffer, typed_array.byteOffset, num_of_bytes));

				return num_of_bytes;
			}
			else {
				throw new Error("Object '" + typed_array + "' is not a typed array");
			} 

		},	
		// Copy the pinned array address from pinned_array allocated on the heap to the typed array.
		// 	 adress of managed pinned array -> copy from heap -> typed array memory
		typedarray_copy_from : function (typed_array, pinned_array, begin, end, bytes_per_element) {

			// JavaScript typed arrays are array-like objects and provide a mechanism for accessing 
			// raw binary data. (...) To achieve maximum flexibility and efficiency, JavaScript typed arrays 
			// split the implementation into buffers and views. A buffer (implemented by the ArrayBuffer object)
			//  is an object representing a chunk of data; it has no format to speak of, and offers no 
			// mechanism for accessing its contents. In order to access the memory contained in a buffer, 
			// you need to use a view. A view provides a context — that is, a data type, starting offset, 
			// and number of elements — that turns the data into an actual typed array.
			// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Typed_arrays
			if (!!(typed_array.buffer instanceof ArrayBuffer && typed_array.BYTES_PER_ELEMENT)) 
			{
				// Some sanity checks of what is being asked of us
				// lets play it safe and throw an error here instead of assuming to much.
				// Better safe than sorry later
				if (bytes_per_element !== typed_array.BYTES_PER_ELEMENT)
					throw new Error("Inconsistent element sizes: TypedArray.BYTES_PER_ELEMENT '" + typed_array.BYTES_PER_ELEMENT + "' sizeof managed element: '" + bytes_per_element + "'");

				// how much space we have to work with
				var num_of_bytes = (end - begin) * bytes_per_element;
				// how much typed buffer space are we talking about
				var view_bytes = typed_array.length * typed_array.BYTES_PER_ELEMENT;
				// only use what is needed.
				if (num_of_bytes > view_bytes)
					num_of_bytes = view_bytes;

				// Create a new view for mapping
				var typedarrayBytes = new Uint8Array(typed_array.buffer, 0, num_of_bytes);
				// offset index into the view
				var offset = begin * bytes_per_element;
				// Set view bytes to value from HEAPU8
				typedarrayBytes.set(Module.HEAPU8.subarray(pinned_array + offset, pinned_array + offset + num_of_bytes));
				return num_of_bytes;
			}
			else {
				throw new Error("Object '" + typed_array + "' is not a typed array");
			} 

		},	
		// Creates a new typed array from pinned array address from pinned_array allocated on the heap to the typed array.
		// 	 adress of managed pinned array -> copy from heap -> typed array memory
		typed_array_from : function (pinned_array, begin, end, bytes_per_element, type) {

			// typed array
			var newTypedArray = 0;

			switch (type)
			{
				case 5: 
					newTypedArray = new Int8Array(end - begin);
					break;
				case 6: 
					newTypedArray = new Uint8Array(end - begin);
					break;
				case 7: 
					newTypedArray = new Int16Array(end - begin);
					break;
				case 8: 
					newTypedArray = new Uint16Array(end - begin);
					break;
				case 9: 
					newTypedArray = new Int32Array(end - begin);
					break;
				case 10: 
					newTypedArray = new Uint32Array(end - begin);
					break;
				case 13: 
					newTypedArray = new Float32Array(end - begin);
					break;
				case 14:
					newTypedArray = new Float64Array(end - begin);
					break;
				case 15:  // This is a special case because the typed array is also byte[]
					newTypedArray = new Uint8ClampedArray(end - begin);
					break;
			}

			this.typedarray_copy_from(newTypedArray, pinned_array, begin, end, bytes_per_element);
			return newTypedArray;
		},
		js_to_mono_enum: function (method, parmIdx, js_obj) {
			this.bindings_lazy_init ();
    
			if (js_obj === null || typeof js_obj === "undefined")
				return 0;
			
			var monoObj, monoEnum;
			try {
				monoObj = MONO.mono_wasm_new_root (this.js_to_mono_obj (js_obj));
				// Check enum contract
				monoEnum = MONO.mono_wasm_new_root (this.call_method (this.object_to_enum, null, "iim!", [ method, parmIdx, monoObj.value ]))
				// return the unboxed enum value.
				return this.mono_unbox_enum (monoEnum.value);
			} finally {
				MONO.mono_wasm_release_roots (monoObj, monoEnum);
			}
		},
		wasm_binding_obj_new: function (js_obj_id, ownsHandle, type)
		{
			return this.call_method (this.bind_js_obj, null, "iii", [js_obj_id, ownsHandle, type]);
		},
		wasm_bind_existing: function (mono_obj, js_id)
		{
			return this.call_method (this.bind_existing_obj, null, "mi", [mono_obj, js_id]);
		},

		wasm_bind_core_clr_obj: function (js_id, gc_handle)
		{
			return this.call_method (this.bind_core_clr_obj, null, "ii", [js_id, gc_handle]);
		},

		wasm_get_js_id: function (mono_obj)
		{
			return this.call_method (this.get_js_id, null, "m", [mono_obj]);
		},

		wasm_get_raw_obj: function (gchandle)
		{
			return this.call_method (this.get_raw_mono_obj, null, "i!", [gchandle]);
		},

		try_extract_mono_obj:function (js_obj) {
			if (js_obj === null || typeof js_obj === "undefined" || typeof js_obj.__mono_gchandle__ === "undefined")
				return 0;
			return this.wasm_get_raw_obj (js_obj.__mono_gchandle__);
		},

		mono_method_get_call_signature: function(method) {
			this.bindings_lazy_init ();

			return this.call_method (this.get_call_sig, null, "i", [ method ]);
		},

		get_task_and_bind: function (tcs, js_obj) {
			var gc_handle = this.mono_wasm_free_list.length ? this.mono_wasm_free_list.pop() : this.mono_wasm_ref_counter++;
			var task_gchandle = this.call_method (this.tcs_get_task_and_bind, null, "oi", [ tcs, gc_handle + 1 ]);
			js_obj.__mono_gchandle__ = task_gchandle;
			this.mono_wasm_object_registry[gc_handle] = js_obj;
			this.free_task_completion_source(tcs);
			tcs.is_mono_tcs_task_bound = true;
			js_obj.__mono_bound_tcs__ = tcs.__mono_gchandle__;
			tcs.__mono_bound_task__ = js_obj.__mono_gchandle__;
			return this.wasm_get_raw_obj (js_obj.__mono_gchandle__);
		},

		free_task_completion_source: function (tcs) {
			if (tcs.is_mono_tcs_result_set)
			{
				this.call_method (this.unbind_raw_obj_and_free, null, "ii", [ tcs.__mono_gchandle__ ]);
			}
			if (tcs.__mono_bound_task__)
			{
				this.call_method (this.unbind_raw_obj_and_free, null, "ii", [ tcs.__mono_bound_task__ ]);
			}
		},

		extract_mono_obj: function (js_obj) {
			if (js_obj === null || typeof js_obj === "undefined")
				return 0;

			var result = null;
			var gc_handle = js_obj.__mono_gchandle__;
			if (gc_handle) {
				result = this.wasm_get_raw_obj (gc_handle);

				// It's possible the managed object corresponding to this JS object was collected,
				//  in which case we need to make a new one.
				if (!result) {
					delete js_obj.__mono_gchandle__;
					delete js_obj.is_mono_bridged_obj;
				}
			}

			if (!result) {
				gc_handle = this.mono_wasm_register_obj(js_obj);
				result = this.wasm_get_raw_obj (gc_handle);
			}

			return result;
		},

		extract_js_obj: function (mono_obj) {
			if (mono_obj == 0)
				return null;

			var js_id = this.wasm_get_js_id (mono_obj);
			if (js_id > 0)
				return this.mono_wasm_require_handle(js_id);

			var gcHandle = this.mono_wasm_free_list.length ? this.mono_wasm_free_list.pop() : this.mono_wasm_ref_counter++;
			var js_obj = {
				__mono_gchandle__: this.wasm_bind_existing(mono_obj, gcHandle + 1),
				is_mono_bridged_obj: true
			};

			this.mono_wasm_object_registry[gcHandle] = js_obj;
			return js_obj;
		},

		_createNamedFunction: function (name, argumentNames, body, closure) {
			var result = null, keys = null, closureArgumentList = null, closureArgumentNames = null;

			if (closure) {
				closureArgumentNames = Object.keys (closure);
				closureArgumentList = new Array (closureArgumentNames.length);
				for (var i = 0, l = closureArgumentNames.length; i < l; i++)
					closureArgumentList[i] = closure[closureArgumentNames[i]];
			}

			var constructor = this._createRebindableNamedFunction (name, argumentNames, body, closureArgumentNames);
			result = constructor.apply (null, closureArgumentList);

			return result;
		},

		_createRebindableNamedFunction: function (name, argumentNames, body, closureArgNames) {
			var strictPrefix = "\"use strict\";\r\n";
			var uriPrefix = "", escapedFunctionIdentifier = "";

			if (name) {
				uriPrefix = "//# sourceURL=https://mono-wasm.invalid/" + name + "\r\n";
				escapedFunctionIdentifier = name;
			} else {
				escapedFunctionIdentifier = "unnamed";
			}

			var rawFunctionText = "function " + escapedFunctionIdentifier + "(" +
				argumentNames.join(", ") +
				") {\r\n" +
				body +
				"\r\n};\r\n";

			var lineBreakRE = /\r(\n?)/g;

			rawFunctionText = 
				uriPrefix + strictPrefix + 
				rawFunctionText.replace(lineBreakRE, "\r\n    ") + 
				"    return " + escapedFunctionIdentifier + ";\r\n";

			var result = null, keys = null;

			if (closureArgNames) {
				keys = closureArgNames.concat ([rawFunctionText]);
			} else {
				keys = [rawFunctionText];
			}

			result = Function.apply (Function, keys);
			return result;
		},

		_create_primitive_converters: function () {
			var result = new Map ();
			result.set ('m', { steps: [{ }], size: 0});
			result.set ('s', { steps: [{ convert: this.js_string_to_mono_string.bind (this)}], size: 0, needsRoot: true });
			result.set ('o', { steps: [{ convert: this.js_to_mono_obj.bind (this)}], size: 0, needsRoot: true });
			result.set ('u', { steps: [{ convert: this.js_to_mono_uri.bind (this)}], size: 0, needsRoot: true });
			// FIXME: The signature of js_to_mono_enum is incompatible - it should be (obj, method, parmIdx);
			result.set ('k', { steps: [{ convert: this.js_to_mono_enum.bind (this), indirect: 'i64'}], size: 8});
			result.set ('j', { steps: [{ convert: this.js_to_mono_enum.bind (this), indirect: 'i32'}], size: 8});
			result.set ('i', { steps: [{ indirect: 'i32'}], size: 8});
			result.set ('l', { steps: [{ indirect: 'i64'}], size: 8});
			result.set ('f', { steps: [{ indirect: 'float'}], size: 8});
			result.set ('d', { steps: [{ indirect: 'double'}], size: 8});
			return this._primitive_converters = result;
		},

		_create_converter_for_marshal_string: function (args_marshal) {
			// console.log("_create_converter_for_marshal_string", args_marshal);

			var primitiveConverters = this._primitive_converters;
			if (!primitiveConverters)
				primitiveConverters = this._create_primitive_converters ();

			var steps = [];
			var size = 0;
			var is_result_definitely_unmarshaled = false,
				is_result_possibly_unmarshaled = false,
				result_unmarshaled_if_argc = -1;

			for (var i = 0; i < args_marshal.length; ++i) {
				var key = args_marshal[i];

				if (i === args_marshal.length - 1) {
					if (key === "!") {
						is_result_definitely_unmarshaled = true;
						continue;
					} else if (key === "m") {
						is_result_possibly_unmarshaled = true;
						result_unmarshaled_if_argc = args_marshal.length - 1;
					}
				} else if (key === "!")
					throw new Error ("! must be at the end of the signature");

				var conv = primitiveConverters.get (key);
				if (!conv)
					throw new Error ("Unknown parameter type " + type);

				var localStep = Object.create (conv.steps[0]);
				localStep.size = conv.size;
				localStep.key = args_marshal[i];
				steps.push (localStep);
				size += conv.size;
			}

			return { 
				steps: steps, size: size, args_marshal: args_marshal, 
				is_result_definitely_unmarshaled: is_result_definitely_unmarshaled,
				is_result_possibly_unmarshaled: is_result_possibly_unmarshaled,
				result_unmarshaled_if_argc: result_unmarshaled_if_argc
			};
		},

		_get_converter_for_marshal_string: function (args_marshal) {
			// console.log("_get_converter_for_marshal_string", args_marshal);

			if (!this._signature_converters)
				this._signature_converters = new Map();

			var converter = this._signature_converters.get (args_marshal);
			if (!converter) {
				converter = this._create_converter_for_marshal_string (args_marshal);
				this._signature_converters.set (args_marshal, converter);
			}

			// console.log("result.args_marshal", converter.args_marshal);

			return converter;
		},

		_compile_converter_for_marshal_string: function (args_marshal) {
			// console.log("_compile_converter_for_marshal_string", args_marshal);

			var converter = this._get_converter_for_marshal_string (args_marshal);
			if (!converter.args_marshal)
				throw new Error ("Corrupt converter");

			if (converter.compiled_function && converter.compiled_variadic_function)
				return converter;

			var converterName = args_marshal.replace("!", "_result_unmarshaled");
			
			var body = [];
			var argumentNames = ["rootBuffer", "method"];

			// worst-case allocation size instead of allocating dynamically, plus padding
			var bufferSizeBytes = converter.size + (args_marshal.length * 4) + 16;
			var rootBufferSize = args_marshal.length;
			// ensure the indirect values are 8-byte aligned so that aligned loads and stores will work
			var indirectBaseOffset = ((((args_marshal.length * 4) + 7) / 8) | 0) * 8;

			var closure = {
				converter: converter
			};
			var indirectLocalOffset = 0;

			body.push (
				"var buffer = Module._malloc (" + bufferSizeBytes + ");",
				"var indirectStart = buffer + " + indirectBaseOffset + ";",
				"var indirect32 = (indirectStart / 4) | 0, indirect64 = (indirectStart / 8) | 0;",
				"var buffer32 = (buffer / 4) | 0;",
				"var valueAddress = 0;",
				""
			);

			for (var i = 0; i < converter.steps.length; i++) {
				var step = converter.steps[i];
				var closureKey = "step" + i;
				var valueKey = "value" + i;
				var hasAssignedAddress = false;

				var argKey = "arg" + i;
				argumentNames.push (argKey);

				if (step.convert) {
					closure[closureKey] = step.convert;
					body.push ("console.log('calling converter '" + step.key + ", " + closureKey + ", 'with value', obj);"); 
					body.push ("var " + valueKey + " = " + closureKey + "(" + argKey + ", method, " + i + ");");
					body.push ("console.log('converter result', " + valueKey + ");");
				} else {
					body.push ("var " + valueKey + " = " + argKey + ";");
					body.push ("console.log('arg" + i + " value', " + valueKey + ");");
				}

				if (step.indirect) {
					hasAssignedAddress = true;

					switch (step.indirect) {
						case "u32":
							body.push ("Module.HEAPU32[indirect32 + " + (indirectLocalOffset / 4) + "] = " + valueKey + ";");
							break;
						case "i32":
							body.push ("Module.HEAP32[indirect32 + " + (indirectLocalOffset / 4) + "] = " + valueKey + ";");
							break;
						case "float":
							body.push ("Module.HEAPF32[indirect32 + " + (indirectLocalOffset / 4) + "] = " + valueKey + ";");
							break;
						case "double":
							body.push ("Module.HEAPF64[indirect64 + " + (indirectLocalOffset / 8) + "] = " + valueKey + ";");
							break;
						case "i64":
							body.push ("Module.setValue (indirectStart + " + indirectLocalOffset + ", " + valueKey + ", 'i64');");					
							break;
						default:
							throw new Error ("Unimplemented indirect type: " + step.indirect);
					}

					body.push ("valueAddress = indirectStart + " + indirectLocalOffset + ";");
					indirectLocalOffset += step.size;
				}

				if (!step.convert && !step.indirect)
					body.push ("valueAddress = " + valueKey + " | 0;");

				if (step.needsRoot)
					body.push ("rootBuffer.set (" + i + ", valueAddress);");

				body.push ("Module.HEAP32[buffer32 + " + i + "] = valueAddress;", "");

				body.push ("console.log ('wrote ptr', valueAddress, 'to address', (buffer32 + " + i + ") * 4);");
			}

			body.push ("console.log ('conversion finished');");

			body.push ("return buffer;");

			var bodyJs = body.join ("\r\n"), compiledFunction = null, compiledVariadicFunction = null;
			try {
				compiledFunction = this._createNamedFunction("converter_" + converterName, argumentNames, bodyJs, closure);
				converter.compiled_function = compiledFunction;
				// console.log("compiled converter", compiledFunction);
			} catch (exc) {
				converter.compiled_function = null;
				console.warn("compiling converter failed for", bodyJs, "with error", exc);
				throw exc;
			}

			argumentNames = ["rootBuffer", "method", "args"];
			closure = {
				converter: compiledFunction
			};
			body = [
				"return converter(",
				"  rootBuffer, method,"
			];

			for (var i = 0; i < converter.steps.length; i++) {
				body.push(
					"  args[" + i + 
					(
						(i == converter.steps.length - 1) 
							? "]" 
							: "], "
					)
				);
			}

			body.push(");");

			bodyJs = body.join ("\r\n");
			try {
				compiledVariadicFunction = this._createNamedFunction("variadic_converter_" + converterName, argumentNames, bodyJs, closure);
				converter.compiled_variadic_function = compiledVariadicFunction;
				// console.log("compiled converter", compiledFunction);
			} catch (exc) {
				converter.compiled_variadic_function = null;
				console.warn("compiling converter failed for", bodyJs, "with error", exc);
				throw exc;
			}

			return converter;
		},

		_verify_args_for_method_call: function (args_marshal, args) {
			var has_args = (typeof args === "object") && args.length > 0;
			var has_args_marshal = typeof args_marshal === "string";

			if (has_args) {
				if (!has_args_marshal)
					throw new Error ("No signature provided for method call.");
				else if (args.length > args_marshal.length)
					throw new Error ("Too many parameter values. Expected at most " + args_marshal.length + " value(s) for signature " + args_marshal);
			}

			return has_args_marshal && has_args;
		},

		_get_args_root_buffer_for_method_call: function (converter) {
			if (!converter)
				return null;

			var result;
			if (converter.scratchRootBuffer) {
				result = converter.scratchRootBuffer;
				converter.scratchRootBuffer = null;
			} else {
				result = MONO.mono_wasm_new_root_buffer (converter.steps.length);
				result.converter = converter;
			}
			return result;
		},

		_release_args_root_buffer_from_method_call: function (argsRootBuffer) {
			if (!argsRootBuffer)
				return;

			var converter = argsRootBuffer.converter;
			// Store the arguments root buffer for re-use in later calls
			if (converter && !converter.scratchRootBuffer && argsRootBuffer) {
				argsRootBuffer.clear ();
				converter.scratchRootBuffer = argsRootBuffer;
			} else {
				argsRootBuffer.release ();
			}
		},

		_handle_possible_exception_for_method_call: function (result, exception) {
			if (exception === 0)
				return;

			var msg = this.conv_string (result);
			var err = new Error (msg); //the convention is that invoke_method ToString () any outgoing exception
			console.warn ("error", msg, "at location", err.stack);
			throw err;
		},

		_maybe_produce_signature_warning: function (converter) {
			if (converter.has_warned_about_signature)
				return;

			console.warn ("MONO_WASM: Deprecated raw return value signature: '" + converter.args_marshal + "'. End the signature with '!' instead of 'm'.");
			converter.has_warned_about_signature = true;
		},

		_decide_if_result_is_marshaled: function (converter, argc) {
			if (!converter)
				return true;

			if (
				converter.is_result_possibly_unmarshaled && 
				(argc === converter.result_unmarshaled_if_argc)
			) {
				if (argc < converter.result_unmarshaled_if_argc)
					throw new Error(["Expected >= ", converter.result_unmarshaled_if_argc, "argument(s) but got", argc, "for signature " + converter.args_marshal].join(" "));

				this._maybe_produce_signature_warning (converter);
				return false;
			} else {
				if (argc < converter.steps.length)
					throw new Error(["Expected", converter.steps.length, "argument(s) but got", argc, "for signature " + converter.args_marshal].join(" "));

				return !converter.is_result_definitely_unmarshaled;
			}
		},

		/*
		args_marshal is a string with one character per parameter that tells how to marshal it, here are the valid values:

		i: int32
		j: int32 - Enum with underlying type of int32
		l: int64 
		k: int64 - Enum with underlying type of int64
		f: float
		d: double
		s: string
		o: js object will be converted to a C# object (this will box numbers/bool/promises)
		m: raw mono object. Don't use it unless you know what you're doing

		to suppress marshaling of the return value, place '!' at the end of args_marshal, i.e. 'ii!' instead of 'ii'
		*/
		call_method: function (method, this_arg, args_marshal, args) {
			this.bindings_lazy_init ();

			// HACK: Sometimes callers pass null or undefined, coerce it to 0 since that's what wasm expects
			this_arg = this_arg | 0;

			var needs_converter = this._verify_args_for_method_call (args_marshal, args);

			var buffer = 0, converter = null, argsRootBuffer = null;
			var is_result_marshaled = true;

			// check if the method signature needs argument mashalling
			if (needs_converter) {
				converter = this._compile_converter_for_marshal_string (args_marshal);

				is_result_marshaled = this._decide_if_result_is_marshaled (converter, args.length);

				// console.log('is_result_marshaled=', is_result_marshaled, 'for argc', args.length, 'and signature ' + converter.args_marshal);
	
				argsRootBuffer = this._get_args_root_buffer_for_method_call (converter);

				console.log ("converting args for", this._get_method_description (method), args_marshal, ":", args);
				buffer = converter.compiled_variadic_function (argsRootBuffer, method, args);
			}

			return this._call_method_with_converted_args (method, this_arg, buffer, is_result_marshaled, argsRootBuffer);
		},

		_handle_exception_and_produce_result_for_call: function (resultRoot, exceptionRoot, is_result_marshaled) {
			this._handle_possible_exception_for_method_call (resultRoot.value, exceptionRoot.value);

			if (is_result_marshaled)
				result = this._unbox_mono_obj_rooted (resultRoot);
			else
				result = resultRoot.value;

			return result;
		},

		_teardown_after_call: function (buffer, resultRoot, exceptionRoot, argsRootBuffer) {
			this._release_args_root_buffer_from_method_call (argsRootBuffer);

			if (buffer)
				Module._free (buffer);

			if (resultRoot)
				resultRoot.release ();
			if (exceptionRoot)
				exceptionRoot.release ();
		},

		_get_method_description: function (method) {
			if (!this._method_descriptions)
				this._method_descriptions = new Map();

			var result = this._method_descriptions.get (method);
			if (!result)
				result = "method#" + method;
			return result;
		},

		_call_method_with_converted_args: function (method, this_arg, buffer, is_result_marshaled, argsRootBuffer) {
			var resultRoot = MONO.mono_wasm_new_root (), exceptionRoot = MONO.mono_wasm_new_root ();
			try {
				resultRoot.value = this.invoke_method (method, this_arg, buffer, exceptionRoot.get_address ());
				var result = this._handle_exception_and_produce_result_for_call (resultRoot, exceptionRoot, is_result_marshaled);
				if (is_result_marshaled)
					console.log(this._get_method_description(method) + " returned (boxed):", result);
				else
					console.log(this._get_method_description(method) + " returned ptr:", result);
				return result;
			} finally {
				this._teardown_after_call (buffer, resultRoot, exceptionRoot, argsRootBuffer);
			}
		},

		bind_method: function (method, this_arg, args_marshal, friendly_name) {
			this.bindings_lazy_init ();

			this_arg = this_arg | 0;

			// console.log ("bind_method", method, this_arg, args_marshal);

			var converter = null;
			if (typeof (args_marshal) === "string")
				converter = this._compile_converter_for_marshal_string (args_marshal);

			if (false)
				return function bound_method () {
					var argsRootBuffer = BINDING._get_args_root_buffer_for_method_call (converter);
					var buffer = 0;
					if (converter)
						buffer = converter.compiled_variadic_function (argsRootBuffer, method, arguments);
		
					var is_result_marshaled = BINDING._decide_if_result_is_marshaled (converter, arguments.length);

					// console.log('is_result_marshaled=', is_result_marshaled, 'for argc', args.length, 'and signature ' + converter.args_marshal + ' (bound)');

					return BINDING._call_method_with_converted_args (method, this_arg, buffer, is_result_marshaled, argsRootBuffer);
				};

			var closure = {
				binding_support: this,
				converter: converter,
				method: method,
				this_arg: this_arg
			};
			var argumentNames = [];
			var body = [];

			if (converter) {
				body.push(
					"var argsRootBuffer = binding_support._get_args_root_buffer_for_method_call (converter);",
					"var buffer = converter.compiled_function (",
					"  argsRootBuffer, method,"
				);

				for (var i = 0; i < converter.steps.length; i++) {
					var argName = "arg" + i;
					argumentNames.push(argName);
					body.push(
						"  " + argName +
						(
							(i == converter.steps.length - 1) 
								? "" 
								: ", "
						)
					);
				}

				body.push(");");
	
			} else {
				body.push("var argsRootBuffer = null, buffer = 0;");
			}

			if (converter.is_result_definitely_unmarshaled) {
				body.push ("var is_result_marshaled = false;");
			} else if (converter.is_result_possibly_unmarshaled) {
				body.push ("var is_result_marshaled = arguments.length !== " + converter.result_unmarshaled_if_argc + ";");
				// body.push ("console.log('is_result_marshaled=', is_result_marshaled, 'for argc', arguments.length, 'and signature " + converter.args_marshal + " (bound)');");
			} else {
				body.push ("var is_result_marshaled = true;");
			}

			body.push ("return binding_support._call_method_with_converted_args (method, this_arg, buffer, is_result_marshaled, argsRootBuffer);");

			bodyJs = body.join ("\r\n");

			if (friendly_name) {
				var escapeRE = /[^A-Za-z0-9_]/g;
				friendly_name = friendly_name.replace(escapeRE, "_");
			}

			return this._createNamedFunction("bound_method_" + (friendly_name || method) + "_with_this_" + this_arg, argumentNames, bodyJs, closure);
		},

		invoke_delegate: function (delegate_obj, js_args) {
			this.bindings_lazy_init ();

			// Check to make sure the delegate is still alive on the CLR side of things.
			if (typeof delegate_obj.__mono_delegate_alive__ !== "undefined") {
				if (!delegate_obj.__mono_delegate_alive__)
					throw new Error("The delegate target that is being invoked is no longer available.  Please check if it has been prematurely GC'd.");
			}

			var [delegateRoot, argsRoot] = MONO.mono_wasm_new_roots ([this.extract_mono_obj (delegate_obj), undefined]);
			try {
				if (!this.delegate_dynamic_invoke) {
					if (!this.corlib)
						this.corlib = this.assembly_load ("System.Private.CoreLib");
					if (!this.delegate_class)
						this.delegate_class = this.find_class (this.corlib, "System", "Delegate");
					if (!this.delegate_class)
					{
						throw new Error("System.Delegate class can not be resolved.");
					}
					this.delegate_dynamic_invoke = this.find_method (this.delegate_class, "DynamicInvoke", -1);
				}
				argsRoot.value = this.js_array_to_mono_array (js_args);
				if (!this.delegate_dynamic_invoke)
					throw new Error("System.Delegate.DynamicInvoke method can not be resolved.");
				// Note: the single 'm' passed here is causing problems with AOT.  Changed to "mo" again.  
				// This may need more analysis if causes problems again.
				return this.call_method (this.delegate_dynamic_invoke, delegateRoot.value, "mo", [ argsRoot.value ]);
			} finally {
				MONO.mono_wasm_release_roots (delegateRoot, argsRoot);
			}
		},
		
		resolve_method_fqn: function (fqn) {
			this.bindings_lazy_init ();
			
			var assembly = fqn.substring(fqn.indexOf ("[") + 1, fqn.indexOf ("]")).trim();
			fqn = fqn.substring (fqn.indexOf ("]") + 1).trim();

			var methodname = fqn.substring(fqn.indexOf (":") + 1);
			fqn = fqn.substring (0, fqn.indexOf (":")).trim ();

			var namespace = "";
			var classname = fqn;
			if (fqn.indexOf(".") != -1) {
				var idx = fqn.lastIndexOf(".");
				namespace = fqn.substring (0, idx);
				classname = fqn.substring (idx + 1);
			}

			var asm = this.assembly_load (assembly);
			if (!asm)
				throw new Error ("Could not find assembly: " + assembly);

			var klass = this.find_class(asm, namespace, classname);
			if (!klass)
				throw new Error ("Could not find class: " + namespace + ":" + classname + " in assembly " + assembly);

			var method = this.find_method (klass, methodname, -1);
			if (!method)
				throw new Error ("Could not find method: " + methodname);
			return method;
		},

		call_static_method: function (fqn, args, signature) {
			this.bindings_lazy_init ();

			var method = this.resolve_method_fqn (fqn);

			if (typeof signature === "undefined")
				signature = Module.mono_method_get_call_signature (method);

			return this.call_method (method, null, signature, args);
		},

		bind_static_method: function (fqn, signature) {
			this.bindings_lazy_init ();

			var method = this.resolve_method_fqn (fqn);

			if (typeof signature === "undefined")
				signature = Module.mono_method_get_call_signature (method);

			return BINDING.bind_method (method, null, signature, fqn);
		},
		
		bind_assembly_entry_point: function (assembly) {
			this.bindings_lazy_init ();

			var asm = this.assembly_load (assembly);
			if (!asm)
				throw new Error ("Could not find assembly: " + assembly);

			var method = this.assembly_get_entry_point(asm);
			if (!method)
				throw new Error ("Could not find entry point for assembly: " + assembly);

			if (typeof signature === "undefined")
				signature = Module.mono_method_get_call_signature (method);

			return function() {
				return BINDING.call_method (method, null, signature, arguments);
			};
		},
		call_assembly_entry_point: function (assembly, args, signature) {
			this.bindings_lazy_init ();

			var asm = this.assembly_load (assembly);
			if (!asm)
				throw new Error ("Could not find assembly: " + assembly);

			var method = this.assembly_get_entry_point(asm);
			if (!method)
				throw new Error ("Could not find entry point for assembly: " + assembly);

			if (typeof signature === "undefined")
				signature = Module.mono_method_get_call_signature (method);

			return this.call_method (method, null, signature, args);
		},
		// Object wrapping helper functions to handle reference handles that will
		// be used in managed code.
		mono_wasm_register_obj: function(obj) {

			var gc_handle = undefined;
			if (obj !== null && typeof obj !== "undefined") 
			{
				gc_handle = obj.__mono_gchandle__;

				if (typeof gc_handle === "undefined") {
					var handle = this.mono_wasm_free_list.length ?
								this.mono_wasm_free_list.pop() : this.mono_wasm_ref_counter++;
					obj.__mono_jshandle__ = handle;
					// Obtain the JS -> C# type mapping.
					var wasm_type = obj[Symbol.for("wasm type")];
					obj.__owns_handle__ = true;
					gc_handle = obj.__mono_gchandle__ = this.wasm_binding_obj_new(handle + 1, obj.__owns_handle__, typeof wasm_type === "undefined" ? -1 : wasm_type);
					this.mono_wasm_object_registry[handle] = obj;
						
				}
			}
			return gc_handle;
		},
		mono_wasm_require_handle: function(handle) {
			if (handle > 0)
				return this.mono_wasm_object_registry[handle - 1];
			return null;
		},
		mono_wasm_unregister_obj: function(js_id) {
			var obj = this.mono_wasm_object_registry[js_id - 1];
			if (typeof obj  !== "undefined" && obj !== null) {
				// if this is the global object then do not
				// unregister it.
				if (globalThis === obj)
					return obj;

				var gc_handle = obj.__mono_gchandle__;
				if (typeof gc_handle  !== "undefined") {

					obj.__mono_gchandle__ = undefined;
					obj.__mono_jshandle__ = undefined;

					// If we are unregistering a delegate then mark it as not being alive
					// this will be checked in the delegate invoke and throw an appropriate
					// error.
					if (typeof obj.__mono_delegate_alive__ !== "undefined")
						obj.__mono_delegate_alive__ = false;

					this.mono_wasm_object_registry[js_id - 1] = undefined;
					this.mono_wasm_free_list.push(js_id - 1);
				}
			}
			return obj;
		},
		mono_wasm_free_handle: function(handle) {
			this.mono_wasm_unregister_obj(handle);
		},
		mono_wasm_free_raw_object: function(js_id) {
			var obj = this.mono_wasm_object_registry[js_id - 1];
			if (typeof obj  !== "undefined" && obj !== null) {
				// if this is the global object then do not
				// unregister it.
				if (globalThis === obj)
					return obj;

				var gc_handle = obj.__mono_gchandle__;
				if (typeof gc_handle  !== "undefined") {

					obj.__mono_gchandle__ = undefined;
					obj.__mono_jshandle__ = undefined;

					this.mono_wasm_object_registry[js_id - 1] = undefined;
					this.mono_wasm_free_list.push(js_id - 1);
				}
			}
			return obj;
		},
		mono_wasm_parse_args : function (args) {
			var js_args = this.mono_array_to_js_array(args);
			this.mono_wasm_save_LMF();
			return js_args;
		},
		mono_wasm_save_LMF : function () {
			//console.log("save LMF: " + BINDING.mono_wasm_owned_objects_frames.length)
			BINDING.mono_wasm_owned_objects_frames.push(BINDING.mono_wasm_owned_objects_LMF);
			BINDING.mono_wasm_owned_objects_LMF = undefined;
		},
		mono_wasm_unwind_LMF : function () {
			var __owned_objects__ = this.mono_wasm_owned_objects_frames.pop();
			// Release all managed objects that are loaded into the LMF
			if (typeof __owned_objects__ !== "undefined")
			{
				// Look into passing the array of owned object handles in one pass.
				var refidx;
				for (refidx = 0; refidx < __owned_objects__.length; refidx++)
				{
					var ownerRelease = __owned_objects__[refidx];
					this.call_method(this.safehandle_release_by_handle, null, "i", [ ownerRelease ]);
				}
			}
			//console.log("restore LMF: " + BINDING.mono_wasm_owned_objects_frames.length)

		},
		mono_wasm_convert_return_value: function (ret) {
			this.mono_wasm_unwind_LMF();
			return this.js_to_mono_obj (ret);
		},
	},

	mono_wasm_invoke_js_with_args: function(js_handle, method_name, args, is_exception) {
		BINDING.bindings_lazy_init ();

		var obj = BINDING.get_js_obj (js_handle);
		if (!obj) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		var js_name = BINDING.conv_string (method_name);
		if (!js_name) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid method name object '" + method_name + "'");
		}

		var js_args = BINDING.mono_wasm_parse_args(args);

		var res;
		try {
			var m = obj [js_name];
			if (typeof m === "undefined")
				throw new Error("Method: '" + js_name + "' not found for: '" + Object.prototype.toString.call(obj) + "'");
			var res = m.apply (obj, js_args);
			return BINDING.mono_wasm_convert_return_value(res);
		} catch (e) {
			// make sure we release object reference counts on errors.
			BINDING.mono_wasm_unwind_LMF();
			var res = e.toString ();
			setValue (is_exception, 1, "i32");
			if (res === null || res === undefined)
				res = "unknown exception";
			return BINDING.js_string_to_mono_string (res);
		}
	},
	mono_wasm_get_object_property: function(js_handle, property_name, is_exception) {
		BINDING.bindings_lazy_init ();

		var obj = BINDING.mono_wasm_require_handle (js_handle);
		if (!obj) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		var js_name = BINDING.conv_string (property_name);
		if (!js_name) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid property name object '" + js_name + "'");
		}

		var res;
		try {
			var m = obj [js_name];
			if (m === Object(m) && obj.__is_mono_proxied__)
				m.__is_mono_proxied__ = true;
				
			return BINDING.js_to_mono_obj (m);
		} catch (e) {
			var res = e.toString ();
			setValue (is_exception, 1, "i32");
			if (res === null || typeof res === "undefined")
				res = "unknown exception";
			return BINDING.js_string_to_mono_string (res);
		}
	},
    mono_wasm_set_object_property: function (js_handle, property_name, value, createIfNotExist, hasOwnProperty, is_exception) {

		BINDING.bindings_lazy_init ();

		var requireObject = BINDING.mono_wasm_require_handle (js_handle);
		if (!requireObject) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		var property = BINDING.conv_string (property_name);
		if (!property) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid property name object '" + property_name + "'");
		}

        var result = false;

		var js_value = BINDING.unbox_mono_obj(value);
		BINDING.mono_wasm_save_LMF();

        if (createIfNotExist) {
            requireObject[property] = js_value;
            result = true;
        }
        else {
			result = false;
			if (!createIfNotExist)
			{
				if (!requireObject.hasOwnProperty(property))
					return false;
			}
            if (hasOwnProperty === true) {
                if (requireObject.hasOwnProperty(property)) {
                    requireObject[property] = js_value;
                    result = true;
                }
            }
            else {
                requireObject[property] = js_value;
                result = true;
            }
        
		}
		BINDING.mono_wasm_unwind_LMF();
        return BINDING.call_method (BINDING.box_js_bool, null, "im", [ result ]);
	},
	mono_wasm_get_by_index: function(js_handle, property_index, is_exception) {
		BINDING.bindings_lazy_init ();

		var obj = BINDING.mono_wasm_require_handle (js_handle);
		if (!obj) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		try {
			var m = obj [property_index];
			return BINDING.js_to_mono_obj (m);
		} catch (e) {
			var res = e.toString ();
			setValue (is_exception, 1, "i32");
			if (res === null || typeof res === "undefined")
				res = "unknown exception";
			return BINDING.js_string_to_mono_string (res);
		}
	},
	mono_wasm_set_by_index: function(js_handle, property_index, value, is_exception) {
		BINDING.bindings_lazy_init ();

		var obj = BINDING.mono_wasm_require_handle (js_handle);
		if (!obj) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		var js_value = BINDING.unbox_mono_obj(value);
		BINDING.mono_wasm_save_LMF();

		try {
			obj [property_index] = js_value;
			BINDING.mono_wasm_unwind_LMF();
			return true;
		} catch (e) {
			var res = e.toString ();
			setValue (is_exception, 1, "i32");
			if (res === null || typeof res === "undefined")
				res = "unknown exception";
			return BINDING.js_string_to_mono_string (res);
		}
	},
	mono_wasm_get_global_object: function(global_name, is_exception) {
		BINDING.bindings_lazy_init ();

		var js_name = BINDING.conv_string (global_name);

		var globalObj = BINDING.mono_wasm_get_global ();

		if (!js_name) {
			globalObj = globalThis;
		}
		else {
			globalObj = globalThis[js_name];
		}

		if (globalObj === null || typeof globalObj === undefined) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Global object '" + js_name + "' not found.");
		}

		return BINDING.js_to_mono_obj (globalObj);
	},
	mono_wasm_release_handle: function(js_handle, is_exception) {
		BINDING.bindings_lazy_init ();

		BINDING.mono_wasm_free_handle(js_handle);
	},	
	mono_wasm_release_object: function(js_handle, is_exception) {
		BINDING.bindings_lazy_init ();

		BINDING.mono_wasm_free_raw_object(js_handle);
	},	
	mono_wasm_bind_core_object: function(js_handle, gc_handle, is_exception) {
		BINDING.bindings_lazy_init ();

		var requireObject = BINDING.mono_wasm_require_handle (js_handle);
		if (!requireObject) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		BINDING.wasm_bind_core_clr_obj(js_handle, gc_handle );
		requireObject.__mono_gchandle__ = gc_handle;
		requireObject.__js_handle__ = js_handle;
		return gc_handle;
	},
	mono_wasm_bind_host_object: function(js_handle, gc_handle, is_exception) {
		BINDING.bindings_lazy_init ();

		var requireObject = BINDING.mono_wasm_require_handle (js_handle);
		if (!requireObject) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		BINDING.wasm_bind_core_clr_obj(js_handle, gc_handle );
		requireObject.__mono_gchandle__ = gc_handle;
		return gc_handle;
	},
	mono_wasm_new: function (core_name, args, is_exception) {
		BINDING.bindings_lazy_init ();

		var js_name = BINDING.conv_string (core_name);

		if (!js_name) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Core object '" + js_name + "' not found.");
		}

		var coreObj = globalThis[js_name];

		if (coreObj === null || typeof coreObj === "undefined") {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("JavaScript host object '" + js_name + "' not found.");
		}

		var js_args = BINDING.mono_wasm_parse_args(args);
		
		try {
			
			// This is all experimental !!!!!!
			var allocator = function(constructor, js_args) {
				// Not sure if we should be checking for anything here
				var argsList = new Array();
				argsList[0] = constructor;
				if (js_args)
					argsList = argsList.concat (js_args);
				var tempCtor = constructor.bind.apply (constructor, argsList);
				var obj = new tempCtor ();
				return obj;
			};
	
			var res = allocator(coreObj, js_args);
			var gc_handle = BINDING.mono_wasm_free_list.length ? BINDING.mono_wasm_free_list.pop() : BINDING.mono_wasm_ref_counter++;
			BINDING.mono_wasm_object_registry[gc_handle] = res;
			return BINDING.mono_wasm_convert_return_value(gc_handle + 1);
		} catch (e) {
			var res = e.toString ();
			setValue (is_exception, 1, "i32");
			if (res === null || res === undefined)
				res = "Error allocating object.";
			return BINDING.js_string_to_mono_string (res);
		}	

	},

	mono_wasm_typed_array_to_array: function(js_handle, is_exception) {
		BINDING.bindings_lazy_init ();

		var requireObject = BINDING.mono_wasm_require_handle (js_handle);
		if (!requireObject) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		return BINDING.js_typed_array_to_array(requireObject);
	},
	mono_wasm_typed_array_copy_to: function(js_handle, pinned_array, begin, end, bytes_per_element, is_exception) {
		BINDING.bindings_lazy_init ();

		var requireObject = BINDING.mono_wasm_require_handle (js_handle);
		if (!requireObject) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		var res = BINDING.typedarray_copy_to(requireObject, pinned_array, begin, end, bytes_per_element);
		return BINDING.js_to_mono_obj (res)
	},
	mono_wasm_typed_array_from: function(pinned_array, begin, end, bytes_per_element, type, is_exception) {
		BINDING.bindings_lazy_init ();
		var res = BINDING.typed_array_from(pinned_array, begin, end, bytes_per_element, type);
		return BINDING.js_to_mono_obj (res)
	},
	mono_wasm_typed_array_copy_from: function(js_handle, pinned_array, begin, end, bytes_per_element, is_exception) {
		BINDING.bindings_lazy_init ();

		var requireObject = BINDING.mono_wasm_require_handle (js_handle);
		if (!requireObject) {
			setValue (is_exception, 1, "i32");
			return BINDING.js_string_to_mono_string ("Invalid JS object handle '" + js_handle + "'");
		}

		var res = BINDING.typedarray_copy_from(requireObject, pinned_array, begin, end, bytes_per_element);
		return BINDING.js_to_mono_obj (res)
	},


};

autoAddDeps(BindingSupportLib, '$BINDING')
mergeInto(LibraryManager.library, BindingSupportLib)
