zombie.controller('LoginCtrl', ['$rootScope', '$scope', '$location', 'config', 'barricade', 'ajax',
    function ($rootScope, $scope, $location, config, barricade, ajax) {
        'use strict';

        // TODO: remove after debugging
        $scope.username = 'Admin1';
        $scope.password = 'Pass123!';

        $scope.config = config;

        $scope.loginRequired = function () {
            return !barricade.expired
                && $location.path() != '/login';
        };

        $scope.login = function () {
            ajax.request({
                request: function () {
                    return barricade.login($scope.username, $scope.password);
                },
                form: $scope.form,
                ladda: 'login',
                onInvalid: function (form) {
                    form.username.$dirty = form.username.$invalid;
                    form.password.$dirty = form.password.$invalid;
                },
                onSuccess: function (data) {
                    config.load(data.config, true);
                    if ($location.path() == '/login') $location.path('/');
                },
                onError: function (data, status) {
                    if (status == 400 && data.activation) {
                        $rootScope.activation.require(data.activation);
                        $location.path('/activation');
                    }
                }
            });
        };

        $scope.cancel = function () {
            $location.path(barricade.lastNoAuth || '/');
        };
    }]);
