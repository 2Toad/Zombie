zombie.directive('zbAngularLocale', ['config',
    function (config) {
        'use strict';

        return {
            restrict: 'E',
            link: function (scope, element) {
                var script = document.createElement('script');
                script.src = '/app/js/vendor/angular-locale_' + config.profile.locale + '.js';
                element.append(script);
            }
        };
    }]);
