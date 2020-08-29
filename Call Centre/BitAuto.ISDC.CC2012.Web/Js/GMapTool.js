; (function () {
    var window = this,
	        GMapTool = window.GMapTool = function (mapCanvas, options) {
	            return new _instance(mapCanvas, options);
	        }

    function _instance(mapCanvas, options) {
        this.createMap(mapCanvas, options);
        this.geocoder = new google.maps.Geocoder();
        //this.disableMarkerDraggable = false;无效
    }

    _instance.prototype = {
        createMap: function (mapCanvas, options) {
            var $t = this;
            if (!google || !google.maps) { return; }
            if (!mapCanvas) { return; }

            if (options.markerPosition) { options.center = options.markerPosition; }
            if (options.staticView) {//view
                options.disableDefaultUI = true;
                options.disableDoubleClickZoom = true;
                options.draggable = false;
                options.keyboardShortcuts = false;
                options.navigationControl = false;
                options.scrollwheel = false;

                $t.disableMarkerDraggable = true;
            }
            //else if (options.dynamicView) {//view
            //}
            else {
                $t.disableMarkerDraggable = options.disableMarkerDraggable ? true : false;
            }

            $t.map = new google.maps.Map(mapCanvas, options);
            if (options.markerPosition) {
                if ($t.theMarker) { $t.theMarker.setMap(null); }
                $t.theMarker = new google.maps.Marker({
                    map: $t.map,
                    position: options.markerPosition,
                    draggable: !$t.disableMarkerDraggable
                });
            }
        },

        bindMarkerEvent: function (callback) {
            var $t = this;
            google.maps.event.addListener($t.map, "click", function (latLng) {//latLng.latlng va wa
                var success = true;
                if (latLng) {
                    if ($t.theMarker) { $t.theMarker.setMap(null); }
                    $t.theMarker = new google.maps.Marker({
                        map: $t.map,
                        position: latLng.latLng,
                        draggable: true
                    });
                }
                else { success = false; }
                if (callback) { callback(success); }
            });
        },

        getMarker: function () { return this.theMarker; },

        getMap: function () { return this.map },

        search: function (address, callback) {
            var $t = this;
            $t.geocoder.geocode({
                'address': address,
                'partialmatch': true
            }, function (results, status) {
                if (status == 'OK' && results.length > 0) {
                    $t.map.fitBounds(results[0].geometry.viewport);
                }
                if (callback) { callback(results, status); }
            });
        }
    };

    window.GMapService = {
        registMapJsLoadedCallback: function (f) {
            if (typeof (f) != "function") { return; }
            //typeof (google) == 'undefined' || typeof (google.maps) == 'undefined' || typeof (google.maps.LatLng) == 'undefined'
            if (!(window.MapJsLoaded || false)) {
                window.MapJsLoadedCallback_ = window.MapJsLoadedCallback_ || new Array();
                window.MapJsLoadedCallback_.push(f);
            }
            else {
                f();
            }
        },

        loadMapJs: function () {
            if (!(window.MapJsLoadedCallback || false)) {
                window.MapJsLoadedCallback = function () {
                    window.MapJsLoaded = true;
                    var a = window.MapJsLoadedCallback_ || null;
                    if (a) {
                        for (f in a) {
                            a[f]();
                            //delete a[f];
                        }
                    }
                };
            }
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = "http://maps.google.com/maps/api/js?sensor=false&callback=MapJsLoadedCallback&_radom=" + new Date();
            document.body.appendChild(script);
        }
    }
})();