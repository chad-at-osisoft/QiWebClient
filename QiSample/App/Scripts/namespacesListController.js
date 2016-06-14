'use strict';
angular.module('qisampleApp')
    .controller('namespacesListController', ['$scope', 'QiService', '$timeout', 'adalAuthenticationService',
function ($scope, QiService, $timeout, adalService){
            $scope.error = "";
            $scope.loadingMessage = "Loading...";
            $scope.namespacesList = [];

            $scope.printSelected = null;
            $scope.selectedNSitem = null;

            var tenantId = adalService.userInfo.profile.tid;

            $scope.populateQiNamespaces = function () {
                QiService.getNamespaces(tenantId).then(function (res1) {
                    $scope.namespacesList = res1.data;
                });
            };

            $scope.nsItemChanged = function () {
                bHasStreams = false;
                $scope.printSelected = $scope.selectedNSitem;

                QiService.getStreams(tenantId, $scope.selectedNSitem).then(function (res1) {
                    $scope.streamList = res1.data;
                    bHasStreams = true;
                });
            }

            var bHasStreams = false;

            $scope.hasStreams = function () {
                return bHasStreams;
            }
        
    }]);
//# sourceMappingURL=streamListController.js.map