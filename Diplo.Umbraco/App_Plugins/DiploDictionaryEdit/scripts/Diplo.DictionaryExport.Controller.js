(function () {
    'use strict';

    angular.module("umbraco").controller("DiploDictionaryExportController",
        function ($scope, $routeParams, diploDictionaryResources) {

            diploDictionaryResources.getLanguages().then(function (response) {
                $scope.languages = response;

            }, function (response) {
                notificationsService.error("Error", "Could not load dictionary languages. Ooops.");
            });

        });
})();