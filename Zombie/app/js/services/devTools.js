/*
    EXAMPLES

    // Latency
    return devTools('foo').latency(5000)();

    // Latency with fake response
    return devTools().latency(750)().then(_.partial(devTools().fake, {foo: 'bar'}));
*/

zombie.factory('devTools', ['$q', function ($q) {
    'use strict';

    console.warn('DevTools injected... I pity the fool!');

    return function (sessionId) {
        var tools = {
            sessionId: sessionId || $.now(),
            latency: simulateLatency,
            fake: fakeResponse
        }
        return tools;

        function simulateLatency(latency) {
            log('Simulating Latency: ' + latency);

            return function (response) {
                var defer = $q.defer();

                _.delay(function () {
                    log('Simulating Latency: finished');
                    defer.resolve(response);
                }, latency);

                return defer.promise;
            }
        }

        function fakeResponse(response) {
            return $q.when(response);
        }

        function log(message) {
            typeof message === 'string'
                ? console.log(tools.sessionId + ' - ' + message)
                : console.log(_.extend(message, { __sessionId: tools.sessionId }));
        }
    }
}]);
