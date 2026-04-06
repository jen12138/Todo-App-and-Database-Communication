"use client";

import { useState } from "react";

type Todo = {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
};

type TodoClientProps = {
  initialTodos: Todo[];
};

export default function TodoClient({ initialTodos }: TodoClientProps) {
  const [todos, setTodos] = useState(initialTodos);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");

  async function handleAddTodo(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    if (!title.trim() || !description.trim()) return;

    const res = await fetch("http://127.0.0.1:5187/api/todos", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        title,
        description,
        isCompleted: false,
      }),
    });

    if (!res.ok) {
      alert("Failed to add todo");
      return;
    }

    const newTodo = await res.json();
    setTodos((prev) => [...prev, newTodo]);
    setTitle("");
    setDescription("");
  }

  async function handleToggleTodo(todo: Todo) {
    const updatedTodo = {
      ...todo,
      isCompleted: !todo.isCompleted,
    };

    const res = await fetch(`http://127.0.0.1:5187/api/todos/${todo.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(updatedTodo),
    });

    if (!res.ok) {
      alert("Failed to update todo");
      return;
    }

    setTodos((prev) =>
      prev.map((item) => (item.id === todo.id ? updatedTodo : item)),
    );
  }

  async function handleDeleteTodo(id: number) {
    const res = await fetch(`http://127.0.0.1:5187/api/todos/${id}`, {
      method: "DELETE",
    });

    if (!res.ok) {
      alert("Failed to delete todo");
      return;
    }

    setTodos((prev) => prev.filter((todo) => todo.id !== id));
  }

  return (
    <div style={{ maxWidth: "900px", margin: "0 auto" }}>
      <form
        onSubmit={handleAddTodo}
        style={{
          backgroundColor: "#ffffff",
          border: "1px solid #e5e7eb",
          borderRadius: "12px",
          padding: "20px",
          marginBottom: "24px",
          boxShadow: "0 4px 14px rgba(0,0,0,0.06)",
        }}
      >
        <h2 style={{ marginTop: 0, color: "#111827" }}>Add New Todo</h2>

        <input
          type="text"
          placeholder="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          style={{
            width: "100%",
            padding: "12px",
            marginBottom: "12px",
            borderRadius: "8px",
            border: "1px solid #d1d5db",
          }}
        />

        <textarea
          placeholder="Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          style={{
            width: "100%",
            padding: "12px",
            marginBottom: "12px",
            borderRadius: "8px",
            border: "1px solid #d1d5db",
            minHeight: "100px",
          }}
        />

        <button
          type="submit"
          style={{
            backgroundColor: "#2563eb",
            color: "#ffffff",
            border: "none",
            borderRadius: "8px",
            padding: "12px 20px",
            cursor: "pointer",
            fontWeight: "bold",
          }}
        >
          Add Todo
        </button>
      </form>

      <div style={{ display: "grid", gap: "16px" }}>
        {todos.map((todo) => (
          <section
            key={todo.id}
            style={{
              backgroundColor: "#ffffff",
              border: "1px solid #e5e7eb",
              borderRadius: "12px",
              padding: "20px",
              boxShadow: "0 4px 14px rgba(0,0,0,0.06)",
            }}
          >
            <h2
              style={{
                margin: "0 0 8px 0",
                fontSize: "28px",
                color: "#111827",
              }}
            >
              {todo.title}
            </h2>

            <p style={{ margin: "0 0 12px 0", color: "#374151" }}>
              {todo.description}
            </p>

            <p
              style={{
                margin: 0,
                fontWeight: "bold",
                color: todo.isCompleted ? "#059669" : "#d97706",
              }}
            >
              Status: {todo.isCompleted ? "Completed" : "Pending"}
            </p>

            <div style={{ display: "flex", gap: "12px", marginTop: "16px" }}>
              <button
                type="button"
                onClick={() => handleToggleTodo(todo)}
                style={{
                  backgroundColor: todo.isCompleted ? "#d97706" : "#059669",
                  color: "#ffffff",
                  border: "none",
                  borderRadius: "8px",
                  padding: "10px 16px",
                  cursor: "pointer",
                  fontWeight: "bold",
                }}
              >
                {todo.isCompleted ? "Mark Pending" : "Mark Completed"}
              </button>

              <button
                type="button"
                onClick={() => handleDeleteTodo(todo.id)}
                style={{
                  backgroundColor: "#dc2626",
                  color: "#ffffff",
                  border: "none",
                  borderRadius: "8px",
                  padding: "10px 16px",
                  cursor: "pointer",
                  fontWeight: "bold",
                }}
              >
                Delete
              </button>
            </div>
          </section>
        ))}
      </div>
    </div>
  );
}
