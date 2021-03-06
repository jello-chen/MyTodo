<template>
  <section class="todoapp">
    <header class="header">
      <h1>My Todo</h1>
      <input class="new-todo" v-model="newTodoTitle" @keyup.enter="createTodo()" placeholder="What needs to be done?" autofocus>
    </header>
    <!-- This section should be hidden by default and shown when there are todos -->
    <section class="main" v-if="todos.length">
      <input class="toggle-all" type="checkbox">
      <label for="toggle-all" @click="toggleAll">Mark all as complete</label>
      <ul class="todo-list">
        <!-- These are here just to show the structure of the list items -->
        <!-- List items should get the class `editing` when editing and `completed` when marked as completed -->
        <div v-for="(todo, index) in todosInView" :key="index">
          <todo-item :todo="todo" @toggleCompleted="toggleCompleted(index)" @removeSelf="removeTodo(index)" />
        </div>
      </ul>
    </section>
    <!-- This footer should hidden by default and shown when there are todos -->
    <todo-footer v-if="todos.length" :itemsLeft="remaining.length" :currentView="currentView" :clearCompleted="clearCompleted" />
  </section>
</template>

<script lang="ts">
import TodoFooter from './TodoFooter.vue';
import TodoHeader from './TodoHeader.vue';
import TodoItem from './TodoItem.vue';
import axios from 'axios';

import Vue from 'vue';
import { AppState, Todo } from '../models';

export default Vue.extend({
  components: {
    TodoItem,
    TodoFooter,
  },

  props: ['currentView'],

  data() {
    const initialState: AppState = {

      // Input box content.
      newTodoTitle: '',

      // Current todo items.
      todos: [
        
      ],
    };

    return initialState;
  },
  mounted(){
    const that = this;
    this.$nextTick(function(){
      axios.get("/api/todos").then(res => {
        that.todos = res.data;
      }).catch(err => {
        console.error(err);
      })
    })
  },
  methods: {
    /**
     * Adds a new Todo to this instance's list, and reset the title.
     */
    createTodo() {
      const that = this;
      const title = this.newTodoTitle.trim();
      if (!title) {
        return;
      }

      axios.post('/api/todos', {
        completed: false,
        title: title
      })
      .then(res => {
          that.todos.unshift(res.data);
      }).catch(err => {
          console.error(err);
      });

      this.newTodoTitle = '';
    },

    /**
     * Removes the Todo at the given index.
     */
    removeTodo(index: number) {
      if (index >= this.todos.length) {
        throw new Error(`Index deletion at ${index} greater than ${this.todos.length}`);
      }
      const that = this;
      const current = this.todos[index];
      axios.delete("/api/todos/"+current.id).then(res => {
        that.todos.splice(index, 1);
      }).catch(err => {
        console.error(err);
      })
    },

    /**
     * Toggle the Todo's `completed` status at a given position.
     */
    toggleCompleted(index: number) {
      const that = this;
      const current = this.todos[index];
      console.log(current);
      axios.put("/api/todos/" + current.id, {
        completed: current.completed
      }).then(res => {
        // that.todos.splice(index, 1, {
        //   ...current,
        //   completed: !current.completed
        // });
      }).catch(err => {
        console.error(err);
      })
      
    },

    /**
     * Toggle the Todo's `completed` status at a given position.
     */
    clearCompleted() {
      axios.patch("/api/todos/", this.completed).then(res => {

      }).catch(err => {
        console.log(err);
      })
      this.todos = this.remaining;
    },

    /**
     * Toggles all todos in one wipe.
     * The `completed` status for all todos is set to `true` if any todo is not completed.
     * It is set to `false` otherwise.
     */
    toggleAll() {
      const stateForAll = this.completed.length !== this.todos.length;

      for (const todo of this.todos) {
        todo.completed = stateForAll;
      }
    }
  },

  computed: {
    todosInView(): Todo[] {
      switch (this.currentView) {
        case 'completed':
          return this.completed;
        case 'active':
          return this.remaining;
        case 'all':
        default:
          return this.todos;
      }
    },
    completed(): Todo[] {
      return this.todos.filter(isCompleted);
    },
    remaining(): Todo[] {
      return this.todos.filter(isNotCompleted);
    },
  },
});

function isCompleted(todo: Todo) {
  return todo.completed;
}

function isNotCompleted(todo: Todo) {
  return !todo.completed;
}

</script>