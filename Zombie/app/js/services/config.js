zombie.factory('config', ['$http', 'localStorageService',
    function ($http, localStorageService) {
        'use strict';

        var service = {},
            defaults = {
                profile: {
                    locale: 'en-US'
                },
                preferences: {
                    rememberMe: false
                }
            };

        return _.extend(service, {
            init: initializeConfiguration,
            load: loadConfiguration,
            save: saveConfiguration,
            clear: clearConfiguration
        });

        function initializeConfiguration(config) {
            restoreDefaults();
            _.extend(service, config);
            var storage = localStorageService.get('config');
            storage && service.load(storage);
        }

        function loadConfiguration(config, persist) {
            _.extend(service, config, { currentLocale: service.profile.locale });
            $http.defaults.headers.common['Accept-Language'] = service.profile.locale;
            persist && service.save();
        }

        function saveConfiguration() {
            localStorageService.set('config', {
                profile: service.profile,
                preferences: service.preferences
            });
        }

        function clearConfiguration() {
            restoreDefaults();
            localStorageService.remove('config');
        }

        function restoreDefaults() {
            service.profile = defaults.profile;
            service.preferences = defaults.preferences;
        }
    }]);
