var app = angular.module('MyApp', ['datatables']);
app.controller('homeCtrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder',
    function ($scope, $http, DTOptionsBuilder, DTColumnBuilder) {
        $scope.dtColumns = [
           // DTColumnBuilder.newColumn("Id", "Feedback ID"),
            DTColumnBuilder.newColumn("Name", "Name"),
            DTColumnBuilder.newColumn("Email", "Email"),
            DTColumnBuilder.newColumn("Comment", "Comments"),
           
        ]

        $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
            url: "/Feedbacks/getdata",
            type: "POST"
        })
        .withPaginationType('full_numbers')
        .withDisplayLength(10);

    }])