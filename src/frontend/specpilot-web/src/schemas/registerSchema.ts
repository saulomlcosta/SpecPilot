import { z } from 'zod';

export const registerSchema = z.object({
  name: z.string().min(2, 'Informe um nome com pelo menos 2 caracteres.'),
  email: z.string().email('Informe um email valido.'),
  password: z.string().min(8, 'A senha deve ter pelo menos 8 caracteres.')
});

export type RegisterFormValues = z.infer<typeof registerSchema>;
