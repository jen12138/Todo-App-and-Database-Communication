const API_BASE_URL = "http://127.0.0.1:5187";
const REQUEST_TIMEOUT_MS = 5000;

export const TODOS_ENDPOINT = `${API_BASE_URL}/api/todos`;

export async function fetchWithTimeout(
  input: string,
  init?: RequestInit,
): Promise<Response> {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), REQUEST_TIMEOUT_MS);

  try {
    return await fetch(input, {
      ...init,
      signal: controller.signal,
    });
  } catch (error) {
    if (error instanceof Error && error.name === "AbortError") {
      throw new Error(
        "Request timed out. Confirm the backend is running on http://127.0.0.1:5187.",
      );
    }

    throw error;
  } finally {
    clearTimeout(timeoutId);
  }
}
