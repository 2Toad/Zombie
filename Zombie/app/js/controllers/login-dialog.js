zombie.controller('LoginDialogCtrl', ['$rootScope', '$scope', '$location', '$modalInstance', 'config', 'ajax', 'barricade',
    function ($rootScope, $scope, $location, $modalInstance, config, ajax, barricade) {
        'use strict';

        /*
            WARNING: HERE BE DRAGONS!
            Use of angular-ui-bootstrap $modalInstance breaks the $scope. Once it's fixed,
            we can rewrite this code to not pass the form as a parameter, and 
            use $scope.form instead (see login.js for implementation).
        */

        // TODO: remove after debugging
        $scope.username = 'Admin1';
        $scope.password = 'Pass123!';

        $scope.config = config;

        $scope.login = function (form) {
            $scope.form = form;

            ajax.request({
                request: function () {
                    return barricade.login(form.username.$modelValue, form.password.$modelValue);
                },
                form: form,
                ladda: 'login',
                onInvalid: function (form) {
                    form.username.$dirty = form.username.$invalid;
                    form.password.$dirty = form.password.$invalid;
                },
                onSuccess: function (data) {
                    config.load(data.config, true);
                    $modalInstance.close();
                },
                onError: function (data, status) {
                    if (status == 400 && data.activation) {
                        $rootScope.activation.require(data.activation);
                        $modalInstance.close();
                        $location.path('/activation');
                    }
                }
            });
        };

        $scope.close = function () {
            $modalInstance.close();
        };
        $scope.register = function () {
            $modalInstance.close();
            $location.path('/register');
        };
        $scope.help = function () {
            toastr.info('TODO');
        };
    }]);
