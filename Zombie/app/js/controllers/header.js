zombie.controller('HeaderCtrl', ['$scope', '$location', '$modal', 'config', 'barricade',
    function ($scope, $location, $modal, config, barricade) {
        'use strict';

        $scope.config = config;

        $scope.login = function () {
            $modal.open({
                templateUrl: '/app/views/login-dialog.html',
                controller: 'LoginDialogCtrl'
            });
        };

        $scope.logout = function () {
            if (!barricade.noAuth) $location.path('/');
            barricade.logout()
                .finally(function () {
                    config.clear();
                });
        };

        $scope.register = function () {
            $location.path('/register');
        };
    }]);
