'use strict';
angular.module('qisampleApp', ['ngRoute',
                               'ui.bootstrap',
                               'AdalAngular']);


angular.module('qisampleApp')

.constant('QI_SERVER_URL', 'https://qi-data.osisoft.com')
.constant('QI_SERVER_APPID', 'https://qihomeprod.onmicrosoft.com/historian')
.constant('QI_SAMPLEWEBAPP_CLIENTID', '0bea46a4-2aa3-4e61-9c41-285b34c1a570')
.constant('QI_SAMPLEWEBAPP_EXTERNAL_CLIENTID', '1eaf349a-6428-4922-b40b-8c19e0ed74bd')
.constant('QI_SAMPLEWEBAPP_PINOVA_CLIENTID', '0bea46a4-2aa3-4e61-9c41-285b34c1a570')


.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', 'QI_SAMPLEWEBAPP_CLIENTID',
    function ($routeProvider, $httpProvider, adalProvider, QI_SAMPLEWEBAPP_CLIENTID) {

    $routeProvider
      .when("/home", {
        controller: "homeCtrl",
        templateUrl: "/App/Views/Home.html",
    }).when("/dashboard", {
        templateUrl: "/App/Views/Dashboard.html",
        requireADLogin: true,
    }).when("/userdata", {
        controller: "userDataCtrl",
        templateUrl: "/App/Views/UserData.html",
    }).otherwise({ redirectTo: "/Home" });

    var endpoints = {
        // Map the location of a request to an API to a the identifier of the associated resource
        'https://qi-data.osisoft.com': 'https://qihomeprod.onmicrosoft.com/historian'
    };

    adalProvider.init(
        {
            instance: 'https://login.microsoftonline.com/',
            clientId: QI_SAMPLEWEBAPP_CLIENTID,
            extraQueryParameter: 'nux=1',
            endpoints: endpoints,
        },
        $httpProvider
        );
   
}]);
