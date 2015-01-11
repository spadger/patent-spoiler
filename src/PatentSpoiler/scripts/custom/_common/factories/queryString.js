///<reference path="../module.js"/>
'use strict';
angular.module('utils').factory('queryString', [
    function() {

        return {
            get:function() {
                var results = {};

                window.location.search.replace(new RegExp("([^?=&]+)(=([^&]*))?", "g"),
                    function ($0, $1, $2, $3) {
                        results[$1] = $3;
                    });

                return results;
            }
        }

    }]);