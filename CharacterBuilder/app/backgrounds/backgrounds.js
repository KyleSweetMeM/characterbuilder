﻿define(function (require) {
    var _i = {
        ko: require('knockout'),
        $: require('jquery'),
        charajax: require('_custom/services/WebAPI'),
        list: require('_custom/services/listmanager'),
        deferred: require('_custom/deferred')
    };

    return function () {
        var self = this;

        /*==================== BASE DATA ====================*/
        self.backgrounds = _i.ko.observableArray([]);

        /*==================== PAGE STATE/FILTERED ITEMS ====================*/
        self.selectedBackground = _i.ko.observable();
        self.viewingDetails = _i.ko.observable(false);
        self.backgroundListToShow = _i.ko.computed(function () {
            var returnList = self.backgrounds();
            return _i.list.sortAlphabeticallyObservables(returnList);
        });

        self.activate = function () {
            return self.getPageData().done(function () {

            });
        };

        self.getPageData = function () {
            var deferred = _i.deferred.create();
            var promise = _i.deferred.waitForAll(self.getBackgroundList());

            promise.done(function () {
                deferred.resolve();
            });

            return deferred;
        };

        self.getBackgroundList = function () {
            var deferred = _i.deferred.create();
            _i.charajax.get('api/background/GetAllBackgrounds', '').done(function (response) {
                var mapped = _i.ko.mapping.fromJS(response);

                self.backgrounds(mapped());
                deferred.resolve();
            });
            return deferred;
        };

        self.viewMoreDetails = function (bgSelected) {
            self.selectedBackground(bgSelected);
            self.viewingDetails(true);
        };

        self.closeMoreDetails = function () {
            self.viewingDetails(false);
        };


    }
});
