///<reference path="app.js" />
///<reference path="userPatentService.js" />

'use strict';
angular.module('user').controller('UserPatentsController', ['$scope', '$window', 'userPatentService', function ($scope, $window, userPatentService) {

    $scope.working = false;
    $scope.user = $window.user;
    $scope.patents = [];
    $scope.totalPatentCount = 'unknown';
    
    var success = function (result) {
        Array.prototype.push.apply($scope.patents, result.items);
        $scope.totalPatentCount = result.count;
    }

    var fail = function (error) {
        alert('ruh-ro');
    }

    $scope.noItems = function() {
        return $scope.patents.length == 0;
    }
    
    $scope.morePatents = function () {
        $scope.working = true;
        
        userPatentService.morePatents($scope.patents.length, $scope.user).then(success, fail).finally(function () { $scope.working = false; });
    }

    $scope.morePatentsAvailable = function() {
        return $scope.patents.length < $scope.totalPatentCount ;
    }


    $scope.morePatents();
}]);