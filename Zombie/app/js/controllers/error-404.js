zombie.controller('Error404Ctrl', ['$scope', '$window',
    function ($scope, $window) {
        'use strict';

        $scope.retry = function () {
            $window.location.reload();
        };
    }]);
