export interface IProblemDetails {
  detail: string;
  status: number | undefined;
  title: string | undefined;
  type: string | undefined;
  'validation-error': string | undefined;
  'not-found': string | undefined;
  errors: object;
}
