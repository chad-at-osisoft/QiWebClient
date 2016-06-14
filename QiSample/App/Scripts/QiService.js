angular.module('qisampleApp')
    .factory('QiService', ['$http', '$q', 'QI_SERVER_URL', function ($http, $q, QI_SERVER_URL) {
        var qiServerUrl = QI_SERVER_URL;
        var url = qiServerUrl;
        var tenantsBase = "/Qi/Tenants";
        var namespacesBase = "/Qi/{0}/Namespaces";
        var typesBase = "/Qi/{0}/{1}/Types";
        var streamsBase = "/Qi/{0}/{1}/Streams";
        var behaviorBase = "/Qi/{0}/{1}/Behaviors";
        var insertSingle = "/Data/InsertValue";
        var insertMultiple = "/Data/InsertValues";
        var getTemplate = "/{0}/Data/GetWindowValues?startIndex={1}&endIndex={2}";
        var getRangeTemplate = "/{0}/Data/GetRangeValues?startIndex={1}&count={2}";
        
                
        return {
            getNamespaces: function (tenantId) {
                var deferred = $q.defer();
                var myurl = url + "/Qi/" + tenantId + "/Namespaces";
                $http({
                    url: myurl,
                    method: 'GET',
                }).then(function (response) {
                    deferred.resolve(response);
                }, function (error) {
                    deferred.reject(error);
                });
                return deferred.promise;
            },

            //get all the streams under the tenant's Qi Service
            getStreams: function (tenantId, namespaceId) {
                var deferred = $q.defer();
                var myurl = url + "/Qi/" + tenantId + "/" + namespaceId + "/Streams";
                $http({
                    url: myurl,
                    method: 'GET',
                }).then(function (response) {
                    deferred.resolve(response);
                }, function (error) {
                    deferred.reject(error);
                });
                return deferred.promise;
            },
            
            getLastValue: function (tenantId, namespaceId, qiStream, start, count) {
                var deferred = $q.defer();
                var myurl = url + "/Qi/" + tenantId + "/" + namespaceId + "/Streams" + "/" + qiStream + "/Data/GetLastValue";
                $http({
                    url: myurl,
                    method: 'GET',
                }).then(function (response) {
                    deferred.resolve(response);
                }, function (error) {
                    deferred.reject(error);
                });
                return deferred.promise;
            }
        };
    }]);
