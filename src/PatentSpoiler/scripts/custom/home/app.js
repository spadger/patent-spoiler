///<reference path="/scripts/custom/_common/factories/recursionHelper.js" />
///<reference path="/scripts/custom/_common/directives/hierrachyView.js" />
window.homeApp = angular.module('home', []);

registerRecursionHelperFactory(window.homeApp);
registerHierachyViewDirective(window.homeApp);