// utils/paramsSerializer.ts
export const serializeParams = (params: Record<string, any>): string => {
  const searchParams = new URLSearchParams();
  Object.entries(params).forEach(([key, value]) => {
    if (value !== undefined && value !== null) {
      const capitalizedKey = key.charAt(0).toUpperCase() + key.slice(1);
      searchParams.append(capitalizedKey, String(value));
    }
  });
  return searchParams.toString();
};