///<reference path="module.js" />
/// <reference path="ItemService.js" />
'use strict';
angular.module('item').controller('AddItemController', ['$scope', '$window', '$modal', 'itemService', function ($scope, $window, $modal, itemService) {

    $scope.working = false;
    $scope.initialCategory = $window.category;
    $scope.item = {attachments:[], categories: {}, name:'', description:'' };
    $scope.submitted = false;
    $scope.working = false;

    if ($scope.initialCategory) {
        $scope.item.categories[$scope.initialCategory] = { Id: $scope.initialCategory };
    }

    $scope.removeCategory = function(id) {
        delete $scope.item.categories[id];
    }


    $scope.filesUploaded = function (attachments) {
        $scope.item.attachments = attachments;
    }

    $scope.add = function() {
        $scope.submitted = true;
        
        if (!$scope.addForm.$valid) {
            $scope.addForm.$setPristine();
            return;
        }

        $scope.working = true;

        var handleSaveResult = function (result) {

            var modalInstance = $modal.open({
                templateUrl: 'saved.html',
                controller: function ($scope, $modalInstance) {

                    $scope.modalInstance = $modalInstance; //getting NRE

                    $scope.ok = $modalInstance.close;
                }
            });

            modalInstance.result.then(function () {
                $window.location.href = '/item/' + result.id;
            });

        };

        var handleError = function () {
            alert('An error occured whilst saving your item');
        };

        itemService.addItem($scope.item)
            .then(handleSaveResult, handleError)
            .finally(function () { $scope.working = false; });
    }
}]);