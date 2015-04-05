zombie.factory('newsletterService', ['$http', function ($http) {
    'use strict';

    var endpoint = '/api/newsletter/';

    return {
        subscribe: subscribe
    };

    function subscribe(email) {
        return $http.post(endpoint + 'subscribe', { email: email });
    }
}]);
