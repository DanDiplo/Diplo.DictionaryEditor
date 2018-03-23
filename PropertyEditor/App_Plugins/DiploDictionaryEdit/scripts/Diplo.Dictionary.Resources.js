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
            },
            uploadFileToServer: function (file) {
                var request = {
                    file: file
                };
                return $http({
                    method: 'POST',
                    url: "Backoffice/DiploDictionary/DictionaryCsv/ImportCsv",
                    headers: { 'Content-Type': undefined },
                    transformRequest: function (data) {
                        var formData = new FormData();
                        formData.append("file", data.file);
                        return formData;
                    },
                    data: request
                }).then(function (response) {
                    if (response) {
                        var fileName = response.data;
                        return fileName;
                    } else {
                        return false;
                    }
                });
            }
        };
    });
})();