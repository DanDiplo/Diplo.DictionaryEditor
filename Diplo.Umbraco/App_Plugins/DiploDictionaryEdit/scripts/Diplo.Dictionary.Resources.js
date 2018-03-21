(function () {
    'use strict';
    angular.module('umbraco.resources').factory('diploDictionaryResources', function ($q, $http, umbRequestHelper) {

        var basePath = "Backoffice/DiploDictionary/DictionaryEditor/";

        return {
            getEntireDictionary: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(basePath + "GetEntireDictionary")
                );
            },
            getLanguages: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(basePath + "GetLanguages")
                );
            },
            updateDictionary: function (data) {
                return umbRequestHelper.resourcePromise(
                    $http.post(basePath + "UpdateDictionary", JSON.stringify(data))
                );
            }
        };
    });
})();