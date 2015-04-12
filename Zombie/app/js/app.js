window.zombie = angular.module('zombieApp', ['ngRoute', 'ngCookies', 'ngSanitize', 'ttLocalizer', 'ttErrorz',
    'ttBarricade', 'ui.bootstrap', 'xtForm', 'LocalStorageModule'
]).config(['$routeProvider', '$locationProvider', 'xtFormConfigProvider',
    function ($routeProvider, $locationProvider, xtFormConfigProvider) {
        $locationProvider.html5Mode(true);

        $routeProvider.when('/', {
            templateUrl: '/app/views/home.html',
            controller: 'DefaultCtrl',
            noAuth: true
        });
        $routeProvider.when('/login', {
            templateUrl: '/app/views/login.html',
            controller: 'LoginCtrl',
            title: 'Login'
        });
        $routeProvider.otherwise({
            templateUrl: '/app/views/error-404.html',
            controller: 'Error404Ctrl',
            title: '404',
            noAuth: true
        });

        toastr.options = {
            'closeButton': true,
            'debug': false,
            'positionClass': 'toast-top-full-width',
            'onclick': toastr.clear(),
            'showDuration': '100',
            'hideDuration': '1500',
            'timeOut': '2500',
            'extendedTimeOut': '1000',
            'showEasing': 'swing',
            'hideEasing': 'linear',
            'showMethod': 'slideDown',
            'hideMethod': 'fadeOut'
        };

        xtFormConfigProvider.setDefaultValidationStrategy('submitted');
    }
]).run(['$rootScope', 'localizer', 'errorz', 'config', 'barricade',
    function ($rootScope, localizer, errorz, config, barricade) {
        config.init({
            profile: {
                locale: 'en-US'
            }
        });

        localizer.init({
            translationsUrl: '/app/i18n/',
            defaultLocale: config.profile.locale,
            errorHandler: function (msg) {
                toastr.error('Unable to load localization file: ' + msg, 'Fatal Error');
            }
        });

        var serverErrorHandler = function () {
            localizer.translateAll('general', ['serverErrorMessage', 'serverErrorTitle'], function (t) {
                toastr.error(t.serverErrorMessage, t.serverErrorTitle);
            }).catch(_.partial(toastr.error, 'Please try again.', 'Server Error'));
        }

        errorz.init({
            0: serverErrorHandler,
            500: serverErrorHandler
        });

        barricade.init(config.preferences.rememberMe, {
            tokenRequestUrl: '/api/oauth/token',
            tokenInvalidateUrl: '/api/oauth/invalidate',
            loginTemplateUrl: '/app/views/login.html',
            serverErrorTemplateUrl: '/app/views/error-500.html',
            exclusions: ['/app/views/form-error.html'],
            serverError: function (rejection) {
                rejection.status === 403 && localizer.translateAll('general', [
                    'unauthorizedAccessMessage', 'unauthorizedAccessTitle'
                ], function (t) {
                    toastr.warning(t.unauthorizedAccessMessage, t.unauthorizedAccessTitle);
                });
                return rejection;
            }
        });

        $rootScope.$on('$routeChangeStart', function (event, next) {
            $rootScope.title = next.title;
        });
    }]);
