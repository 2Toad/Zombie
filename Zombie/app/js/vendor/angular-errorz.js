/*
 * Angular-Errorz
 * Copyright (C)2014 2Toad, LLC.
 * http://2toad.github.io/Angular-Errorz
 * 
 * Version: 1.0.1
 * License: MIT
 */

(function () {
    "use strict";

    angular.module("ttErrorz", [])

    .factory("errorz", [function () {
        var self = {
            handlers: {},
            addHandler: function(errorCode, handler) {
                self.handlers[errorCode] = handler;
            },
            handled: function(errorCode) {
              return self.handlers[errorCode] !== undefined;
            },
            handledErrorCodes: function () {
                return Object.keys(self.handlers);
            },
            init: function (handlers) {
                angular.extend(self.handlers, handlers);
            },
            onError: function(rejection) {
                var handler = self.handlers[rejection.status];
                if (handler !== undefined) {
                    var r = handler(rejection);
                    if (r) rejection = r;
                }
                return rejection;
            }
        };

        return self;
    }])

    .factory("errorz.interceptor", ["$q", "errorz", function ($q, errorz) {
        return {
            responseError: function (rejection) {
                rejection = errorz.onError(rejection);
                return $q.reject(rejection);
            }
        };
    }])

    .config(["$httpProvider", function ($httpProvider) {
        $httpProvider.interceptors.push("errorz.interceptor");
    }]);
}());
