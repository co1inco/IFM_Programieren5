use std::ops;

#[derive(Debug)]
pub struct Point2D {
    pub x: f32,
    pub y: f32,
    e: i32,
}

impl Point2D {
    pub fn new(x: f32, y: f32) -> Self {
        Self {
            x,
            y,
            e: 1,
        }
    }

    pub fn is_point(&self) -> bool {
        self.e == 1
    }
}

fn validate_p(p : &Point2D, v : &Point2D) {
    if !p.is_point() || !v.is_point() { panic!("Faulty data structure"); }
}


impl ops::Sub<Point2D> for Point2D {
    type Output = crate::vector2d::Vector2D;

    fn sub(self, rhs: Point2D) -> Self::Output {
        validate_p(&self, &rhs);
        crate::vector2d::Vector2D::new(self.x - rhs.x, self.y - rhs.y)
    }
}