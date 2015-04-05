zombie.controller('Error500Ctrl', ['$rootScope', '$scope', '$window', '$timeout',
    function ($rootScope, $scope, $window, $timeout) {
        'use strict';

        $rootScope.title = 'Error';
        $scope.time = (new Date).toLocaleString();

        $scope.home = function () {
            $scope.ladda.retry.start();
            $timeout(function () {
                $window.location.reload();
                $scope.ladda.retry.stop();
            }, 1000);
        };
    }]);
