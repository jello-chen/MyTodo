import Vue from 'vue';
import VueRouter from 'vue-router';
import TodoApp from './components/TodoApp.vue';
var viewNames = ['completed', 'active', '*'];
var routes = viewNames.map(function (view) { return ({
    path: '/' + view,
    component: TodoApp,
    props: {
        currentView: view === '*' ? 'all' : view,
    },
}); });
var router = new VueRouter({
    routes: routes,
});
Vue.use(VueRouter);
var app = new Vue({
    el: '#app',
    template: '<router-view></router-view>',
    router: router,
    mixins: [VueRouter],
});
//# sourceMappingURL=index.js.map