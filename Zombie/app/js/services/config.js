/**
 * #init {function()}
 *   Loads the configuration from a cookie
 * 
 * #load {function(config)}
 *   Loads the values contained within the specified config, into the user's profile and preferences.
 *     ~ Pass in just the properties you want to change
 *       ~ Example to change the culture: config.load({preferences: {culture: 'en-UK'}})
 *     ~ Call clear() first if you want to re-load
 *
 *   @param config {object}
 *     - profile {string|object} – the user's profile
 *         - locale {string} - the globaliation locale code
 *     - preferences {string|object} – the user's preferences
 *         - rememberMe {boolean} - whether or not the user's login (access token) should be persisted in a cookie
 *
 * #save {function()}
 *   Saves the configuration to a cookie
 * 
 * #clear {function()}
 *   Removes the configuration from memory and deletes the cookie
**/
zombie.factory('config', ['$http', 'localStorageService',
    function ($http, localStorageService) {
        'use strict';

        var self = {
            profile: {
                language: undefined
            },
            preferences: {
                rememberMe: false
            },
            init: function () {
                var cookie = localStorageService.get('config');
                if (cookie) self.load(cookie);
            },
            load: function (config, persist) {
                $.extend(self.profile, config.profile);
                $.extend(self.preferences, config.preferences);

                if (self.profile.language.locale) $http.defaults.headers.common['Accept-Language'] = self.profile.language.locale;

                if (persist) self.save();
            },
            save: function () {
                localStorageService.set('config', {
                    profile: self.profile,
                    preferences: self.preferences
                });
            },
            clear: function () {
                self.profile = {};
                self.preferences = {};
                localStorageService.remove('config');
            }
        };
        return self;
    }]);
