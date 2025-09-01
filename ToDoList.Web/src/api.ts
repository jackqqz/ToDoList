import axios from "axios";

const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
});

export type ToDoItem = {
    id?: string;
    title: string;
    description?: string;
    isCompleted?: boolean;
    dueDate?: string;          // "YYYY-MM-DD"
    categoryId?: string | null;
    toDoListId?: string;
};

export type ToDoList = {
    id: string;
    title: string;
    items?: ToDoItem[];
};

export type Category = { id: string; name: string };

// Lists
export const createList = (title: string) =>
    api.post<ToDoList>("/api/ToDoLists", null, { params: { title } }).then(r => r.data);

export const getList = (id: string) =>
    api.get<ToDoList>(`/api/ToDoLists/${id}`).then(r => r.data);

// Items for a list
export const addItemToList = (listId: string, item: ToDoItem) =>
    api.post<string>(`/api/ToDoLists/${listId}/items`, item).then(r => r.data);

export const updateToDoItem = (id: string, item: ToDoItem) =>
    api.put<ToDoItem>(`/api/ToDoItem/${id}`, item).then(r => r.data);

export const deleteToDoItem = (id: string) =>
    api.delete<boolean>(`/api/ToDoItem/${id}`).then(r => r.data);

// Categories
export const getCategories = () =>
    api.get<Category[]>("/api/Categories").then(r => r.data);

export const createCategory = (name: string) =>
    api.post<Category>("/api/Categories", null, { params: { name } }).then(r => r.data);