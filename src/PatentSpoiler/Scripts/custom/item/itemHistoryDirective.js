///<reference path="module.js" />
'use strict';
angular.module('item').directive('itemHistory', function() {
    return {
        restrict: 'AE',
        scope: {
            setId: '@',
            ssIi:'@'
        },
        templateUrl: '/scripts/custom/item/itemhistory.html',
        controller:function($scope, itemService) {

            var initialised = false;

            $scope.$on('OpenHistory', function() {
                if (!initialised) {
                    $scope.moreItems();
                    initialised = true;
                }
            });

            $scope.items = [];
            $scope.showMore = true;
            
            $scope.working = false;
            $scope.moreItems = function () {
                $scope.working = true;
                itemService.previousVersions($scope.setId, $scope.items.length)
                           .then(function (result) {

                               for (var i in result.items) {
                                   $scope.items.push(result.items[i]);
                               }

                               if ($scope.items.length >= result.count) {
                                   $scope.showMore = false;
                               }
                    })
                           .finally(function () {
                               $scope.working = false;
                            });
            }
        }
    }
});