///<reference path="../module.js" />
'use strict';
angular.module('utils').directive('fileUploadWrapper', ['FileUploader', function (FileUploader) {
    return {
        restrict: 'AE',
        scope: {
            uploadUri: '&',
            removeUri: '&',
            existingFiles: '&'
        },
        controller: ['$scope','$http', function ($scope, $http) {

            var uploader = $scope.uploader = new FileUploader({
                url: $scope.uploadUri()
            });

            var existing = $scope.existingFiles();
            
            for (var i in existing) {
                var file = existing[i];

                var fileInfo = new FileUploader.FileItem(uploader, file);
                
                fileInfo.progress = 100;
                fileInfo.isUploaded = true;
                fileInfo.isSuccess = true;

                uploader.queue.push(fileInfo);
            }
            
            uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
                console.info('onWhenAddingFileFailed', item, filter, options);
            };
            uploader.onAfterAddingFile = function (fileItem) {
                console.info('onAfterAddingFile', fileItem);
            };
            uploader.onAfterAddingAll = function (addedFileItems) {
                console.info('onAfterAddingAll', addedFileItems);
            };
            uploader.onBeforeUploadItem = function (item) {
                console.info('onBeforeUploadItem', item);
            };
            uploader.onProgressItem = function (fileItem, progress) {
                console.info('onProgressItem', fileItem, progress);
            };
            uploader.onProgressAll = function (progress) {
                console.info('onProgressAll', progress);
            };
            uploader.onSuccessItem = function (fileItem, response, status, headers) {
                console.info('onSuccessItem', fileItem, response, status, headers);
            };
            uploader.onErrorItem = function (fileItem, response, status, headers) {
                console.info('onErrorItem', fileItem, response, status, headers);
            };
            uploader.onCancelItem = function (fileItem, response, status, headers) {
                console.info('onCancelItem', fileItem, response, status, headers);
            };
            uploader.onCompleteItem = function (fileItem, response, status, headers) {
                console.info('onCompleteItem', fileItem, response, status, headers);
            };
            uploader.onCompleteAll = function () {
                console.info('onCompleteAll');
            };

            $scope.uploadAll = function () {
                uploader.uploadAll();
            }

            $scope.remove = function(item) {
                
                $http.delete($scope.removeUri(), { fileName: item.file.name }).then(function () {
                    item.remove();
                }, function () {
                    alert('error');
                });
            }
        }],
        templateUrl: '/scripts/custom/_common/directives/fileUploaderView.html'
    }
}]);