///<reference path="module.js" />
/// <reference path="ItemService.js" />
'use strict';
angular.module('item').controller('EditItemController', ['$scope', '$window', '$modal', 'itemService', function ($scope, $window, $modal, itemService) {
    
    $scope.working = false;
    $scope.item = $window.item;

    var categories = $scope.item.categories;
    $scope.item.categories = {};
    for (var i in categories) {
        var category = categories[i];
        $scope.item.categories[category] = { Id: category };
    }

    $scope.submitted = false;
    $scope.working = false;

    $scope.removeCategory = function(id) {
        delete $scope.item.categories[id];
    }

    $scope.filesUploaded = function (attachments) {
        $scope.item.attachments = attachments;
    }

    $scope.update = function() {
        $scope.submitted = true;
        
        if (!$scope.editForm.$valid) {
            $scope.editForm.$setPristine();
            return;
        }

        $scope.working = true;

        var handleSaveResult = function () {

            var modalInstance = $modal.open({
                templateUrl: 'saved.html',
                controller:function ($scope, $modalInstance) {
    
                    $scope.modalInstance = $modalInstance; //getting NRE

                    $scope.ok = $modalInstance.close;
                }
            });
            
            modalInstance.result.then(function() {
                $window.location.href = '/item/' + $scope.item.id;
            });

        };

        var handleError = function () {
            alert('An error occured whilst saving your item');
        };

        itemService.updateItem($scope.item)
            .then(handleSaveResult, handleError)
            .finally(function () { $scope.working = false; });
    }
}]);