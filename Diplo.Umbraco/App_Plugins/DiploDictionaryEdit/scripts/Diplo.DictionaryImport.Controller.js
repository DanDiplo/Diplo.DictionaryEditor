(function () {
    'use strict';

    angular.module("umbraco").controller("DiploDictionaryImportController",
        function ($scope, $routeParams, notificationsService, fileUploadService) {

            $scope.response = null;
            $scope.file = null;
            $scope.isLoading = false;

            $scope.fileSelected = function (files) {
                $scope.file = files; // In this case, files is just a single path/filename
                $scope.response = null;
            };

            $scope.uploadFile = function () {
                if (!$scope.isLoading) {

                    if ($scope.file) {
                        $scope.isLoading = true;
                        $scope.response = null;

                        fileUploadService.uploadFileToServer($scope.file)
                            .then(function (response) {

                                $scope.response = response;

                                if (response.IsSuccess) {
                                    notificationsService.success("Import Success", response.Message);
                                } else {
                                    notificationsService.error("Import Error", response.Message);
                                }

                                $scope.isLoading = false;
                                $scope.file = null;

                            }, function (response) {

                                $scope.response = response;
                                notificationsService.error("Import Error", "File import failed: " + response.Message);
                                $scope.isLoading = false;
                                $scope.file = null;
                            });
                    } else {
                        notificationsService.error("Error", "You must select a file to upload");
                        $scope.isLoading = false;
                    }
                }
            };
        });

    angular.module("umbraco.resources")
        .factory("fileUploadService", function ($http) {
            return {
                uploadFileToServer: function (file) {
                    var request = {
                        file: file
                    };
                    return $http({
                        method: 'POST',
                        url: "backoffice/DiploDictionary/DictionaryCsv/ImportCsv",
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