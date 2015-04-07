/*
 * Angular-Localizer
 * Copyright (C)2014 2Toad, LLC.
 * http://2toad.github.io/Angular-Localizer
 * 
 * Version: 1.0.0
 * License: MIT
 */

(function () {
    "use strict";

    angular.module("ttLocalizer", [])

    .factory("localizer", ["$q", "$http", function ($q, $http) {
        var service = {
                translationsUrl: "/App/i18n/",
                defaultLocale: "en-US",
                locale: undefined,
                floodDelay: 5000,
                init: initializeService,
                translate: lookupTranslation,
                translateAll: lookupMultipleTranslations,
                errorHandler: undefined
            },
            cache = {},
            lastError = {
                url: undefined,
                time: undefined
            };
        return service;

        function initializeService(config) {
            angular.extend(service, config);
            service.locale = service.defaultLocale;
        }

        function getTranslations(category) {
            var key = category + "." + service.locale;
            var loaded = cache[key];
            return loaded ? $q.when(loaded) : loadTranslations(key);

            function loadTranslations(key) {
                var fileUrl = service.translationsUrl + key + ".js";
                var deferred = $q.defer();

                $http({ url: fileUrl, cache: true })
                    .success(function (data) {
                        cache[key] = data;
                        deferred.resolve(data);
                    })
                    .error(function (data, status) {
                        defaultErrorHandler(fileUrl, data, status);
                        deferred.reject();
                    });

                return deferred.promise;
            }
        }

        function lookupTranslation(key) {
            return function (category, name, callback) {
                return getTranslations(category).then(
                    function (data) {
                        var translation = data.filter(function (t) {
                            return t.name === name;
                        })[0];
                        return translation ? translation[key || "value"] : "{TRANSLATION_MISSING}";
                    }
                ).then(callback || $q.when);
            }
        }

        function lookupMultipleTranslations(category, names, callback) {
            var promises = names.map(function (n) {
                return lookupTranslation()(category, n);
            });

            return $q.all(promises).then(function (t) {
                callback(convertToObject(names, t));
            });

            function convertToObject(names, values) {
                var result = {};
                for (var i = 0; i < names.length; i++) {
                    result[names[i]] = values[i];
                }
                return result;
            }
        }

        function defaultErrorHandler(fileUrl, data, status) {
            if (!preventMessageFlooding()) {
                service.errorHandler
                    ? service.errorHandler(fileUrl, data, status)
                    : console.error("Angular-Localizer Default Error Handler\r\n\r\n" +
                        "Unable to load localization file: " + fileUrl + "\r\n" +
                        "Status:" + status);
            }

            function preventMessageFlooding() {
                if (lastError.url === fileUrl && lastError.time > now()) {
                    return true;
                } else {
                    lastError.url = fileUrl;
                    lastError.time = now(service.floodDelay);
                    return false;
                }

                function now(duration) {
                    return (new Date()).getTime() + (duration || 0);
                }
            }
        }
    }])

    .directive("i18n", ["localizer", function (localizer) {
        return {
            restrict: "E",
            link: function (scope, $element, attrs) {
                localizer.translate()(attrs.category, attrs.name, function (t) {
                    $element.append(t);
                });
            }
        };
    }])

    .directive("i18n", ["localizer", function (localizer) {
        return {
            restrict: "A",
            link: function (scope, $element, attrs) {
                var cn = attrs.i18n.split(".");
                var output = attrs.i18nOutput || "text";

                localizer.translate()(cn[0], cn[1], function (t) {
                    if (attrs.i18nTarget) scope[attrs.i18nTarget] = t;
                    else if (output == "text") $element.append(t);
                    else $element.attr(output, t);
                });
            }
        };
    }]);
}());
