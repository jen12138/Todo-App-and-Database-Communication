import TodoClient from "./TodoClient";

type Todo = {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
};

async function getTodos(): Promise<Todo[]> {
  const res = await fetch("http://127.0.0.1:5187/api/todos", {
    cache: "no-store",
  });

  if (!res.ok) {
    throw new Error("Failed to fetch todos");
  }

  return res.json();
}

export default async function Home() {
  const todos = await getTodos();

  return (
    <main
      style={{
        minHeight: "100vh",
        padding: "40px 24px",
        backgroundColor: "#f7f8fb",
        fontFamily: "Arial, sans-serif",
      }}
    >
      <div style={{ maxWidth: "900px", margin: "0 auto", marginBottom: "32px" }}>
        <h1 style={{ fontSize: "40px", marginBottom: "8px", color: "#111827" }}>
          Next.js Todo List
        </h1>
        <p style={{ marginBottom: "32px", color: "#4b5563" }}>
          Todos fetched from the .NET Web API backend.
        </p>
      </div>

      <TodoClient initialTodos={todos} />
    </main>
  );
}
