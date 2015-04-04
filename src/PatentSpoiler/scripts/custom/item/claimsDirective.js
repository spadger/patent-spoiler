///<reference path="module.js" />
/// <reference path="../../lodash.js" />
(function () {
    'use strict';

    angular.module('item').directive('claims', [function () {

        return{
            restrict: 'E',
            scope: {
                claims:'='
            },
            
            controller: ['$scope', function($scope) {

                $scope.claimObjects = _.map($scope.claims, function(x) { return { value: x } });
                
                $scope.current = '';

                $scope.remove = function(index) {
                    $scope.claimObjects.splice(index, 1);
                };
                $scope.add = function () {
                    if(_.some($scope.claimObjects, function(x){ return x.value === $scope.current })){
                        alert('this claim already exists');
                    } else {
                        $scope.claimObjects.push({ value: $scope.current });
                        $scope.current = '';
                    }
                };

                $scope.$watch('claimObjects', function (arr) {
                    $scope.claims = _.map(arr, 'value');
                }, true);
            }],
            templateUrl: '/scripts/custom/item/claims.html'
        };
    }]);
})();