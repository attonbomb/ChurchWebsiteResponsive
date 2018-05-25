ko.bindingHandlers.toggleAll = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var aa = allBindingsAccessor();
        var selector = aa.selector;
        var va = valueAccessor();
        var value = ko.utils.unwrapObservable(va);
        if (value) {
            $(selector).children().removeAttr('disabled');
        }
        else {
            $(selector).children().attr('disabled', 'disabled');
        }
    }
};

function ViewModel() {
    var self = this;

    self.option = ko.observable(true);

    self.toggle = function () {
        var current = self.option();
        self.option(!current);
    };
}

var vm = new ViewModel();
ko.applyBindings(vm);