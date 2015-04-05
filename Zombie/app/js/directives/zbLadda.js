zombie.directive('zbLadda', function () {
    'use strict';

    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.ladda = scope.ladda || {};
            addLadda(scope.ladda, attrs.zbLadda);

            if (!attrs.style) attrs.$set('data-style', 'zoom-in');
            if (!attrs.size) attrs.$set('data-size', 's');

            element.addClass('ladda-button');

            var ladda;
            scope.$watch('ladda.' + attrs.zbLadda + '.active', function (val) {
                if (val === true) {
                    ladda = ladda || Ladda.create(element[0]);
                    ladda.start();
                }
                else if (ladda) ladda.stop();
            });

            function addLadda(scopeLadda, name) {
                var model = {
                    active: false
                };

                scopeLadda[name] = _.extend(model, {
                    start: startLadda,
                    stop: stopLadda
                });

                function startLadda() {
                    model.active = true;
                }

                function stopLadda() {
                    model.active = false;
                }
            }
        }
    };
});
