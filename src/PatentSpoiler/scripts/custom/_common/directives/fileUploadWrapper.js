///<reference path="../../../lodash.js" />
/// <reference path="../filters/prettyFileSize.js" />
///<reference path="../module.js" />
'use strict';
angular.module('utils').directive('fileUploadWrapper', ['FileUploader', function (FileUploader) {
    return {
        restrict: 'AE',
        scope: {
            uploadUri: '&',
            existingFiles: '&',
            successCallback: '&'
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
                fileInfo.uploadResult = file;
                
                uploader.queue.push(fileInfo);
            }

            if (existing.length > 0) {
                uploader.progress = 100;
            }
            
            $scope.cancel = function () {
                alert('Done');
            }

            $scope.done = function () {
                var allItemsUploaded = _.every(uploader.queue, function(item) {
                    return item.isSuccess && item.isUploaded;
                });
                if (allItemsUploaded) {

                    var results = _.map(uploader.queue, function(item) {

                        return item.uploadResult;
                    });

                    $scope.successCallback()(results);
                } else {
                    alert('Not all files have been successfully uploaded.  Please check or remove those highlighted');
                }
            }

            uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
                alert('Sorry, this file could not be added');
            };
            
            uploader.onErrorItem = function (fileItem, response, status, headers) {
                alert('Sorry, there was an error uploading ' + fileItem);
            };
            
            uploader.onCompleteItem = function (fileItem, response, status, headers) {
                fileItem.uploadResult = response;
            };
            
            $scope.uploadAll = function () {
                uploader.uploadAll();
            }
        }],
        templateUrl: '/scripts/custom/_common/directives/fileUploaderView.html'
    }
}]);