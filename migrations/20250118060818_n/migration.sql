-- CreateTable
CREATE TABLE "Post" (
    "id" TEXT NOT NULL,
    "content" TEXT,
    "rating" INTEGER,
    "feedback" TEXT,
    "pair_left" TEXT,
    "pair_right" TEXT,
    "pairNested_left" TEXT,
    "pairNested_right" TEXT,
    "pairJson" TEXT,

    CONSTRAINT "Post_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "User" (
    "id" TEXT NOT NULL,
    "content" TEXT,
    "rating" INTEGER,
    "feedback" TEXT,
    "pair_left" TEXT,
    "pair_right" TEXT,
    "pairNested_left" TEXT,
    "pairNested_right" TEXT,
    "pairJson" TEXT,

    CONSTRAINT "User_pkey" PRIMARY KEY ("id")
);
