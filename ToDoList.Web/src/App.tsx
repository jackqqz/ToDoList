import { useEffect, useMemo, useState } from "react";
import type { ToDoList, ToDoItem, Category } from "./api";
import {
    createList, getList, addItemToList,
    updateToDoItem, deleteToDoItem,
    getCategories, createCategory
} from "./api";
import "./index.css";

type ModalState =
    | { open: false }
    | {
        open: true;
        list: ToDoList;
        title: string;
        description: string;
        dueDate: string;
        categoryId: string | "";
        showRequired: boolean;
    };

export default function App() {
    const [lists, setLists] = useState<ToDoList[]>([]);
    const [expanded, setExpanded] = useState<Set<string>>(new Set());
    const [newListTitle, setNewListTitle] = useState("");
    const [categories, setCategories] = useState<Category[]>([]);
    const [modal, setModal] = useState<ModalState>({ open: false });

    // recent 10 categories (latest at the front)
    const recent10 = useMemo(
        () => categories.slice(-10).reverse(),
        [categories]
    );

    useEffect(() => {
        const saved = JSON.parse(localStorage.getItem("listIds") ?? "[]") as string[];
        if (saved.length) {
            Promise.all(saved.map(id => getList(id))).then(setLists).catch(() => setLists([]));
        }
        getCategories().then(setCategories).catch(() => setCategories([]));
    }, []);

    useEffect(() => {
        localStorage.setItem("listIds", JSON.stringify(lists.map(l => l.id)));
    }, [lists]);

    function toggleCard(id: string) {
        setExpanded(prev => {
            const next = new Set(prev);
            if (next.has(id)) {
                next.delete(id);
            } else {
                next.add(id);
            }
            return next;
        });
    }

    async function handleCreateList() {
        let title = newListTitle.trim();
        if (!title) { title = "UnnamedList"};
        const created = await createList(title);
        setLists(prev => [created, ...prev]);
        setExpanded(prev => new Set(prev).add(created.id));
        setNewListTitle("");
    }

    // open modal for a specific list
    function openAddTaskModal(list: ToDoList) {
        setModal({
            open: true,
            list,
            title: "",
            description: "",
            dueDate: "",
            categoryId: "",
            showRequired: false,
        });
    }

    function closeModal() {
        setModal({ open: false });
    }

    async function saveTask() {
        if (!modal.open) return;
        const { list, title, description, dueDate, categoryId } = modal;

        const parsedDate = new Date(dueDate).toISOString().substring(0, 10);

        if (!title.trim()) {
            setModal({ ...modal, showRequired: true });
            return;
        }

        const payload: ToDoItem = {
            title: title.trim(),
            description: description || undefined,
            dueDate: parsedDate,
            categoryId: categoryId || undefined,
        };

        await addItemToList(list.id, payload);
        const fresh = await getList(list.id);
        setLists(prev => prev.map(l => (l.id === list.id ? fresh : l)));
        closeModal();
    }


    async function toggleComplete(listId: string, item: ToDoItem) {
        if (!item.id) return;
        const updated: ToDoItem = { ...item, isCompleted: !item.isCompleted, toDoListId: listId };
        await updateToDoItem(item.id, updated);
        const fresh = await getList(listId);
        setLists(prev => prev.map(l => (l.id === listId ? fresh : l)));
    }

    async function removeItem(listId: string, item: ToDoItem) {
        if (!item.id) return;
        await deleteToDoItem(item.id);
        const fresh = await getList(listId);
        setLists(prev => prev.map(l => (l.id === listId ? fresh : l)));
    }

    // create a new category from modal
    async function addNewCategory(name: string) {
        const trimmed = name.trim();
        if (!trimmed) return;
        const created = await createCategory(trimmed);
        const cats = await getCategories();
        setCategories(cats);
        // auto-select the freshly created category
        if (modal.open) setModal({ ...modal, categoryId: created.id });
    }

    return (
        <div className="page light">
            <div className="stack">
                {/* Header / Create list card */}
                <div className="card">
                    <h1 className="title">To-Do List <span className="spark">📝</span></h1>
                    <div className="create-row">
                        <input
                            className="pill-input"
                            placeholder="List title"
                            value={newListTitle}
                            onChange={e => setNewListTitle(e.target.value)}
                            onKeyDown={e => e.key === "Enter" && handleCreateList()}
                        />
                        <button className="pill-btn" onClick={handleCreateList}>Create</button>
                    </div>
                </div>

                {/* Lists as cards */}
                {lists.map(list => {
                    const open = expanded.has(list.id);
                    return (
                        <div key={list.id} className={`card list-card ${open ? "open" : ""}`}>
                            <button className="list-head" onClick={() => toggleCard(list.id)}>
                                <div className="dot" />
                                <div className="list-head-txt">
                                    <div className="list-name">{list.title}</div>
                                    <div className="list-sub">
                                        {list.items?.filter(i => !i.isCompleted).length ?? 0} to do • {list.items?.length ?? 0} total
                                    </div>
                                </div>
                                <div className={`chev ${open ? "up" : "down"}`} aria-hidden />
                            </button>

                            {open && (
                                <div className="list-body">
                                    {/* Items */}
                                    <ul className="items">
                                        {list.items?.map(i => (
                                            <li key={i.id} className="item">
                                                <button
                                                    className={`check ${i.isCompleted ? "checked" : ""}`}
                                                    onClick={() => toggleComplete(list.id, i)}
                                                    aria-label={i.isCompleted ? "Mark as incomplete" : "Mark as complete"}
                                                />
                                                <span className={`item-title ${i.isCompleted ? "done" : ""}`}>{i.title}</span>
                                                <button className="del" onClick={() => removeItem(list.id, i)} aria-label="Delete">×</button>
                                            </li>
                                        ))}
                                        {!list.items?.length && <li className="empty">No items yet</li>}
                                    </ul>

                                    {/* Add Task button (opens modal) */}
                                    <div style={{ display: "flex", justifyContent: "flex-end", paddingTop: 12 }}>
                                        <button className="pill-btn" onClick={() => openAddTaskModal(list)}>Add task</button>
                                    </div>
                                </div>
                            )}
                        </div>
                    );
                })}
            </div>

            {/* ADD TASK MODAL */}
            {modal.open && (
                <div className="modal-backdrop" onClick={closeModal}>
                    <div className="modal" onClick={e => e.stopPropagation()} role="dialog" aria-modal="true">
                        <div className="modal-header">
                            <h3>Add a task to <span className="emph">{modal.list.title}</span></h3>
                            <button className="x" onClick={closeModal} aria-label="Close">×</button>
                        </div>

                        <div className="modal-body">
                            <label className="lbl">Title</label>
                            <input
                                className={`txt ${modal.showRequired && !modal.title.trim() ? "error" : ""}`}
                                placeholder="Task title"
                                value={modal.title}
                                onChange={e => setModal({ ...modal, title: e.target.value, showRequired: false })}
                            />
                            {modal.showRequired && !modal.title.trim() && (
                                <div className="req">* This field is required</div>
                            )}

                            <label className="lbl">Description</label>
                            <textarea
                                className="txt"
                                placeholder="Details (optional)"
                                rows={3}
                                value={modal.description}
                                onChange={e => setModal({ ...modal, description: e.target.value })}
                            />

                            <label className="lbl">Due date</label>
                            <input
                                type="date"
                                className="txt"
                                value={modal.dueDate}
                                onChange={e => setModal({ ...modal, dueDate: e.target.value })}
                            />

                            <label className="lbl">Category</label>
                            <div className="chips">
                                <button
                                    className={`chip ${!modal.categoryId ? "sel" : ""}`}
                                    onClick={() => setModal({ ...modal, categoryId: "" })}
                                >(none)</button>
                                {recent10.map(c => (
                                    <button
                                        key={c.id}
                                        className={`chip ${modal.categoryId === c.id ? "sel" : ""}`}
                                        onClick={() => setModal({ ...modal, categoryId: c.id })}
                                    >
                                        {c.name}
                                    </button>
                                ))}
                            </div>

                            <div className="add-cat">
                                <input
                                    className="txt"
                                    placeholder="Add new category"
                                    onKeyDown={async e => {
                                        if (e.key === "Enter") {
                                            const val = (e.target as HTMLInputElement).value;
                                            await addNewCategory(val);
                                            (e.target as HTMLInputElement).value = "";
                                        }
                                    }}
                                />
                                <button
                                    className="pill-btn"
                                    onClick={async () => {
                                        const inp = document.querySelector<HTMLInputElement>(".add-cat .txt");
                                        if (inp) { await addNewCategory(inp.value); inp.value = ""; }
                                    }}
                                >Add</button>
                            </div>
                        </div>

                        <div className="modal-footer">
                            <button className="ghost" onClick={closeModal}>Cancel</button>
                            <button className="pill-btn" onClick={saveTask}>Save</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
