
merge_sort:
    ; args:
    ;   int index1:     dword [rbp-04]
    ;   int index2:     dword [rbp-08]
    ;   int i.0:        dword [rbp-12]
    ;   int j:          dword [rbp-16]
    ;   int i.1:        dword [rbp-20]
    ;   int m:          dword [rbp-24]
    ;   int *builder:   qword [rbp-32]
    ;   int *arr:       qword [rbp-40]
    ;   int size:       dword [rbp-44]
    ;   int start:      dword [rbp-48]
    ;   int end:        dword [rbp-52]
    push    rbp
    mov     rbp, rsp
    sub     rsp, 64 ; stack pointer must be aligned to 16 bytes for Sys V ABI
    ; move arguments to memory locations
    mov     QWORD PTR [rbp-40], rdi
    mov     DWORD PTR [rbp-44], esi
    mov     DWORD PTR [rbp-48], edx
    mov     DWORD PTR [rbp-52], ecx

    ; if start < 0 goto .L16
    cmp     DWORD PTR [rbp-48], 0
    js      .L16
    ; if start - size >= 0 goto .L16
    mov     eax, DWORD PTR [rbp-48]
    cmp     eax, DWORD PTR [rbp-44]
    jge     .L16
    ; if end < 0 goto .L16
    cmp     DWORD PTR [rbp-52], 0
    js      .L16
    ; if end - size > 0 goto .L16
    mov     eax, DWORD PTR [rbp-52]
    cmp     eax, DWORD PTR [rbp-44]
    jg      .L16
    ; if end != 0 goto .L5
    cmp     DWORD PTR [rbp-52], 0
    jne     .L5
    ; end = size
    mov     eax, DWORD PTR [rbp-44]
    mov     DWORD PTR [rbp-52], eax
.L5:
    ; if end - start <= 1 goto .L17
    mov     eax, DWORD PTR [rbp-52]
    sub     eax, DWORD PTR [rbp-48]
    cmp     eax, 1
    jle     .L17

    mov     edx, DWORD PTR [rbp-48] ; edx = start
    mov     eax, DWORD PTR [rbp-52] ; eax = end
    add     eax, edx ; eax = start + end
    mov     edx, eax ; edx = start + end
    shr     edx, 31 ; edx = sign bit
    add     eax, edx ; add 1 if start + end is negative
    sar     eax ; divide by 2
    mov     DWORD PTR [rbp-24], eax ; m = 
    mov     ecx, DWORD PTR [rbp-24]
    mov     edx, DWORD PTR [rbp-48]
    mov     esi, DWORD PTR [rbp-44]
    mov     rax, QWORD PTR [rbp-40]
    mov     rdi, rax
    call    merge_sort
    mov     ecx, DWORD PTR [rbp-52]
    mov     edx, DWORD PTR [rbp-24]
    mov     esi, DWORD PTR [rbp-44]
    mov     rax, QWORD PTR [rbp-40]
    mov     rdi, rax
    call    merge_sort
    mov     eax, DWORD PTR [rbp-52]
    sub     eax, DWORD PTR [rbp-48]
    cdqe
    sal     rax, 2
    mov     rdi, rax
    call    malloc
    mov     QWORD PTR [rbp-32], rax
    mov     eax, DWORD PTR [rbp-48]
    mov     DWORD PTR [rbp-4], eax
    mov     eax, DWORD PTR [rbp-24]
    mov     DWORD PTR [rbp-8], eax
    mov     DWORD PTR [rbp-12], 0
    jmp     .L7
.L13:
    mov     eax, DWORD PTR [rbp-4]
    cmp     eax, DWORD PTR [rbp-24]
    jge     .L8
    mov     eax, DWORD PTR [rbp-8]
    cmp     eax, DWORD PTR [rbp-52]
    jge     .L8
    mov     eax, DWORD PTR [rbp-4]
    cdqe
    lea     rdx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-40]
    add     rax, rdx
    mov     edx, DWORD PTR [rax]
    mov     eax, DWORD PTR [rbp-8]
    cdqe
    lea     rcx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-40]
    add     rax, rcx
    mov     eax, DWORD PTR [rax]
    cmp     edx, eax
    jg      .L9
    mov     eax, DWORD PTR [rbp-4]
    lea     edx, [rax+1]
    mov     DWORD PTR [rbp-4], edx
    cdqe
    lea     rdx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-40]
    add     rax, rdx
    mov     eax, DWORD PTR [rax]
    jmp     .L10
.L9:
    mov     eax, DWORD PTR [rbp-8]
    lea     edx, [rax+1]
    mov     DWORD PTR [rbp-8], edx
    cdqe
    lea     rdx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-40]
    add     rax, rdx
    mov     eax, DWORD PTR [rax]
.L10:
    mov     edx, DWORD PTR [rbp-12]
    movsx   rdx, edx
    lea     rcx, [0+rdx*4]
    mov     rdx, QWORD PTR [rbp-32]
    add     rdx, rcx
    mov     DWORD PTR [rdx], eax
    jmp     .L11
.L8:
    mov     eax, DWORD PTR [rbp-4]
    cmp     eax, DWORD PTR [rbp-24]
    jl      .L12
    mov     eax, DWORD PTR [rbp-8]
    lea     edx, [rax+1]
    mov     DWORD PTR [rbp-8], edx
    cdqe
    lea     rdx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-40]
    add     rax, rdx
    mov     edx, DWORD PTR [rbp-12]
    movsx   rdx, edx
    lea     rcx, [0+rdx*4]
    mov     rdx, QWORD PTR [rbp-32]
    add     rdx, rcx
    mov     eax, DWORD PTR [rax]
    mov     DWORD PTR [rdx], eax
    jmp     .L11
.L12:
    mov     eax, DWORD PTR [rbp-4]
    lea     edx, [rax+1]
    mov     DWORD PTR [rbp-4], edx
    cdqe
    lea     rdx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-40]
    add     rax, rdx
    mov     edx, DWORD PTR [rbp-12]
    movsx   rdx, edx
    lea     rcx, [0+rdx*4]
    mov     rdx, QWORD PTR [rbp-32]
    add     rdx, rcx
    mov     eax, DWORD PTR [rax]
    mov     DWORD PTR [rdx], eax
.L11:
    add     DWORD PTR [rbp-12], 1
.L7:
    mov     eax, DWORD PTR [rbp-52]
    sub     eax, DWORD PTR [rbp-48]
    cmp     DWORD PTR [rbp-12], eax
    jl      .L13
    mov     DWORD PTR [rbp-16], 0
    mov     eax, DWORD PTR [rbp-48]
    mov     DWORD PTR [rbp-20], eax
    jmp     .L14
.L15:
    mov     eax, DWORD PTR [rbp-16]
    lea     edx, [rax+1]
    mov     DWORD PTR [rbp-16], edx
    cdqe
    lea     rdx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-32]
    add     rax, rdx
    mov     edx, DWORD PTR [rbp-20]
    movsx   rdx, edx
    lea     rcx, [0+rdx*4]
    mov     rdx, QWORD PTR [rbp-40]
    add     rdx, rcx
    mov     eax, DWORD PTR [rax]
    mov     DWORD PTR [rdx], eax
    add     DWORD PTR [rbp-20], 1
.L14:
    mov     eax, DWORD PTR [rbp-20]
    cmp     eax, DWORD PTR [rbp-52]
    jl      .L15
    mov     rax, QWORD PTR [rbp-32]
    mov     rdi, rax
    call    free
    jmp     .L1
.L16:
    nop
    jmp     .L1
.L17:
    nop
.L1:
    leave
    ret
fill_arr_rand:
    push    rbp
    mov     rbp, rsp
    push    rbx
    sub     rsp, 40
    mov     QWORD PTR [rbp-40], rdi
    mov     DWORD PTR [rbp-44], esi
    mov     DWORD PTR [rbp-20], 0
    jmp     .L19
.L20:
    mov     eax, DWORD PTR [rbp-20]
    cdqe
    lea     rdx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-40]
    lea     rbx, [rdx+rax]
    call    rand
    mov     DWORD PTR [rbx], eax
    add     DWORD PTR [rbp-20], 1
.L19:
    mov     eax, DWORD PTR [rbp-20]
    cmp     eax, DWORD PTR [rbp-44]
    jl      .L20
    nop
    nop
    mov     rbx, QWORD PTR [rbp-8]
    leave
    ret
.LC0:
    .string "RAND_MAX: %d\n"
.LC4:
    .string "Trial %d time (ms): %f\n"
.LC6:
    .string "Average for 100 trials (ms): %f\n"
main:
    push    rbp
    mov     rbp, rsp
    sub     rsp, 48
    mov     edi, 0
    call    time
    mov     edi, eax
    call    srand
    mov     esi, 2147483647
    mov     edi, OFFSET FLAT:.LC0
    mov     eax, 0
    call    printf
    mov     DWORD PTR [rbp-24], 5000000
    mov     eax, DWORD PTR [rbp-24]
    cdqe
    mov     esi, 4
    mov     rdi, rax
    call    calloc
    mov     QWORD PTR [rbp-32], rax
    mov     DWORD PTR [rbp-4], 0
    jmp     .L22
.L23:
    mov     eax, DWORD PTR [rbp-4]
    cdqe
    lea     rdx, [0+rax*4]
    mov     rax, QWORD PTR [rbp-32]
    add     rdx, rax
    mov     eax, DWORD PTR [rbp-4]
    mov     DWORD PTR [rdx], eax
    add     DWORD PTR [rbp-4], 1
.L22:
    mov     eax, DWORD PTR [rbp-4]
    cmp     eax, DWORD PTR [rbp-24]
    jl      .L23
    pxor    xmm0, xmm0
    movsd   QWORD PTR [rbp-16], xmm0
    mov     DWORD PTR [rbp-20], 0
    jmp     .L24
.L25:
    call    clock
    mov     QWORD PTR [rbp-40], rax
    mov     esi, DWORD PTR [rbp-24]
    mov     rax, QWORD PTR [rbp-32]
    mov     ecx, 0
    mov     edx, 0
    mov     rdi, rax
    call    merge_sort
    call    clock
    sub     rax, QWORD PTR [rbp-40]
    mov     QWORD PTR [rbp-40], rax
    pxor    xmm0, xmm0
    cvtsi2sd        xmm0, QWORD PTR [rbp-40]
    movsd   xmm2, QWORD PTR .LC2[rip]
    movapd  xmm1, xmm0
    divsd   xmm1, xmm2
    movsd   xmm0, QWORD PTR .LC3[rip]
    mulsd   xmm0, xmm1
    movsd   QWORD PTR [rbp-48], xmm0
    mov     rdx, QWORD PTR [rbp-48]
    mov     eax, DWORD PTR [rbp-20]
    movq    xmm0, rdx
    mov     esi, eax
    mov     edi, OFFSET FLAT:.LC4
    mov     eax, 1
    call    printf
    movsd   xmm0, QWORD PTR [rbp-48]
    movsd   xmm1, QWORD PTR .LC5[rip]
    divsd   xmm0, xmm1
    movsd   xmm1, QWORD PTR [rbp-16]
    addsd   xmm0, xmm1
    movsd   QWORD PTR [rbp-16], xmm0
    add     DWORD PTR [rbp-20], 1
.L24:
    cmp     DWORD PTR [rbp-20], 99
    jle     .L25
    mov     rax, QWORD PTR [rbp-16]
    movq    xmm0, rax
    mov     edi, OFFSET FLAT:.LC6
    mov     eax, 1
    call    printf
    mov     rax, QWORD PTR [rbp-32]
    mov     rdi, rax
    call    free
    mov     eax, 0
    leave
    ret
.LC2:
    .long   0
    .long   1093567616
.LC3:
    .long   0
    .long   1083129856
.LC5:
    .long   0
    .long   1079574528