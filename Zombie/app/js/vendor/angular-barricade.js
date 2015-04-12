/*
 * Barricade-Angular
 * Copyright (C)2014 2Toad, LLC.
 * http://2toad.github.io/Barricade-Angular
 * 
 * Version: 1.4.0
 * License: MIT
 */

(function () {
    "use strict";

    angular.module("barricade", [])

    .factory("barricade", ["$http", "$cookieStore", "$rootScope", "$location", function ($http, $cookieStore, $rootScope, $location) {
        var self = {
            tokenRequestUrl: undefined,
            tokenInvalidateUrl: undefined,
            loginTemplateUrl: undefined,
            serverErrorTemplateUrl: undefined,
            exclusions: [],
            noAuth: undefined,
            authorized: undefined,
            expired: undefined,
            lastNoAuth: undefined,
            init: function (rememberMe, config) {
                angular.extend(self, config);
                formatExclusions();

                $rootScope.$on("$routeChangeStart", function (event, next, current) {
                    self.noAuth = next.noAuth;
                    self.lastNoAuth = current
                        ? current.noAuth
                            ? current.originalPath
                            : self.lastNoAuth
                        : next.noAuth
                            ? next.originalPath
                            : self.lastNoAuth;
                });

                if (rememberMe) {
                    var cookie = $cookieStore.get("barricade");
                    if (cookie) {
                        if (cookie.expiration < now()) {
                            self.setStatus(420);
                        } else {
                            setHeader(cookie.token);
                            self.setStatus(200);
                        }
                    }
                }
            },
            login: function (username, password, tokenRequestUrl) {
                return $http.post(
                    tokenRequestUrl || self.tokenRequestUrl,
                    {Username: username, Password: password}
                ).success(function (data) {
                    var cookie = {
                        token: data.access_token,
                        expiration: now() + (data.expires_in * 1000)
                    };

                    setHeader(cookie.token);
                    $cookieStore.put("barricade", cookie);

                    self.setStatus(200);

                    if (self.reload) {
                        // TODO: We append a hash to the path so Angular will reload 
                        // the view. This works, but it's kind of hacky.
                        $location.hash(now());
                        self.reload = false;
                    }
                });
            },
            logout: function (tokenInvalidateUrl) {
                var promise = $http.delete(tokenInvalidateUrl || self.tokenInvalidateUrl);
                promise.finally(function () {
                    delete $http.defaults.headers.common.Authorization;
                    $cookieStore.remove("barricade");
                    self.setStatus(401);
                });
                return promise;
            },
            setStatus: function (status) {
                self.authorized = status === 200;
                self.expired = status === 420;
            },
            isAuthorized: function () {
                return self.authorized === true || self.noAuth === true;
            }
        };

        function formatExclusions() {
            // Exclude Barricade API URL's
            self.exclusions.push(self.serverErrorTemplateUrl);
            self.exclusions.push(self.loginTemplateUrl);
            self.exclusions.push(self.tokenRequestUrl);

            var formatted = [];
            angular.forEach(self.exclusions, function (value) {
                formatted.push(value.toLowerCase());
            });
            self.exclusions = formatted;
        }

        function setHeader(bearerToken) {
            $http.defaults.headers.common.Authorization = "Bearer " + bearerToken;
        }

        function now() {
            return (new Date()).getTime();
        }

        // TODO: We add a reference to $rootScope to get around the dependency recursion that
        // is caused when trying to inject barricade into the barricade.interceptor service.
        // It's hacky, and it made me throw up in my mouth a little, but it works.
        $rootScope.barricade = self;

        return self;
    }])

    .factory("barricade.interceptor", ["$rootScope", "$q", function ($rootScope, $q) {
        return {
            request: function (config) {
                // Authorized?
                if ($rootScope.barricade.isAuthorized()
                        // Skip excluded URL's
                        || $rootScope.barricade.exclusions.indexOf(config.url.toLowerCase()) > -1) {
                    return config || $q.when(config);
                }

                // Not authorized, so flag it for a reload after a successful login()
                // and block this request from going to the server
                $rootScope.barricade.reload = true;
                return $q.reject(401);
            },
            responseError: function (rejection) {
                if ($rootScope.barricade.serverError) { rejection = $rootScope.barricade.serverError(rejection); }
                if (rejection.status === 401) { $rootScope.barricade.setStatus(401); }
                return $q.reject(rejection);
            }
        };
    }])

    .config(["$httpProvider", function ($httpProvider) {
        $httpProvider.interceptors.push("barricade.interceptor");
    }]);
}());
