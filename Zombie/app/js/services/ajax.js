/**
 *   A service which takes a single configuration object that is used to process an HTTP 
 *   request and returns a promise.
 * 
 *   config {object}
 *     - request {function} - The specified function is expected to handle the HTTP request and return a promise.
 *     - form {object} [optional] - a $scope.form to associate with the request.
 *     - ladda {object} [optional] - the $scope.ladda to associate with the request.
 *     - progress {number} [optional] - the number of milliseconds to wait before showing a progress dialog. Leave undefined for no dialog.
**/
zombie.factory('ajax', ['$q', 'errorz', 'barricade', '$modal',
    function ($q, errorz, barricade, $modal) {
        'use strict';

        return function (config) {
            var progress = {};

            return (config.validator || $q.when)()
                .catch(_.partial(customValidationFailed, config.form))
                .then(_.partial(validateForm, config.form))
                .then(_.partial(processRequest, config.request, config.form, config.ladda, config.progress));

            function customValidationFailed(form, error) {
                form.validationSummary = { message: error };
                return $q.reject();
            }

            function validateForm(form) {
                resetFormValidation(form);
                return form && form.$invalid ? $q.reject('Form validation failed') : $q.when();
            }

            function resetFormValidation(form) {
                if (!form) return;
                form.validationSummary = undefined;
                form.$setPristine(true);
            }

            function processRequest(request, form, ladda, progressDelay) {
                ladda.start();
                toggleProgressDialog(progressDelay);

                return request()
                    .success(_.partial(resetFormValidation, form))
                    .error(function (data, status) {
                        // The status code is handled by Angular-Errorz
                        if (errorz.handled(status)) return;
                        // These status codes are handled by Angular-Barricade
                        if (barricade.statusHandled(status)) return;

                        if (getValidationErrors(form, data)) return;

                        toastr.error(_.isString(data) ? data : _.result(data, 'message') || status, 'Unhandled Error');
                    })
                    .finally(function () {
                        ladda.stop();
                        toggleProgressDialog();
                    });

                function toggleProgressDialog(delay) {
                    typeof delay == 'number' ? showDialog(delay) : closeDialog();

                    function showDialog(delay) {
                        progress.timer = setTimeout(function () {
                            progress.dialog = $modal.open({
                                templateUrl: "/app/views/progress-dialog.html",
                                backdrop: "static"
                            });
                        }, delay);
                    }

                    function closeDialog() {
                        if (!progress.timer) return;
                        clearTimeout(progress.timer);
                        progress.timer = undefined;
                        if (progress.dialog) progress.dialog.close();
                    }
                }

                function getValidationErrors(form, data) {
                    if (!form || !_.has(data, 'errors')) return false;

                    form.validationSummary = {
                        message: getValidationMessage(data)
                    };

                    return _.extend(form.validationSummary, {
                        list: getValidationList(data, form.validationSummary.message)
                    });

                    function getValidationMessage(data) {
                        return _.isString(data.errors)
                            ? data.errors
                            : !_.isObject(data.errors) || _.isEmpty(_.keys(data.errors))
                                ? 'An unexpected error has occurred. Please try again.'
                                : undefined;
                    }

                    function getValidationList(data, message) {
                        return message == undefined
                            ? data.errors
                            : undefined;
                    }
                }
            }
        }
    }
]);
