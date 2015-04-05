zombie.directive('zbValidationSummary', function () {
    return {
        restrict: 'E',
        templateUrl: '/app/views/validation-summary.html',
        link: function (scope) {
            scope.form = scope.form || {};

            _.extend(scope.form, {
                closeValidationSummary: _.partial(closeValidationSummary, scope.form)
            });

            function closeValidationSummary(form) {
                form.validationSummary = undefined;
            };
        }
    };
});
