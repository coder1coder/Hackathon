export function checkValue<T>(value: T): T | null {
  if (typeof value === 'string' && value === '') {
    return null;
  }
  if (value === undefined) {
    return null;
  }
  return value;
}
