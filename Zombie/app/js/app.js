window.zombie = angular.module('zombieApp', ['ngRoute', 'ngCookies', 'ngSanitize', 'ttErrorz', 'barricade', 'ui.bootstrap', 'xtForm', 'LocalStorageModule'])

.config(['$routeProvider', '$locationProvider', 'xtFormConfigProvider',
    function ($routeProvider, $locationProvider, xtFormConfigProvider) {
        // Don't add hashbang to the URL
        $locationProvider.html5Mode(true);

        // Routes
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
])

.run(['$rootScope', 'errorz', 'config', 'barricade',
    function ($rootScope, errorz, config, barricade) {

        var serverErrorHandler = function () { toastr.error('Please try again.', 'Server Error'); };
        errorz.init({
            0: serverErrorHandler,
            500: serverErrorHandler
        });

        config.init();

        barricade.init(config.preferences.rememberMe, {
            tokenRequestUrl: '/api/oauth/token',
            tokenInvalidateUrl: '/api/oauth/invalidate',
            loginTemplateUrl: '/app/views/login.html',
            serverErrorTemplateUrl: '/app/views/error-500.html',
            exclusions: ['/app/views/form-error.html'],
            serverError: function (rejection) {
                if (rejection.status === 403) toastr.warning('You don\'t have permission to perform the requested action.');
                return rejection;
            }
        });

        $rootScope.$on('$routeChangeStart', function (event, next) {
            $rootScope.title = next.title;
        });
    }]);