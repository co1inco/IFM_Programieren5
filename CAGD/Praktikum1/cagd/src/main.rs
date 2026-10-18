mod point2d;
mod vector2d;
mod obj_parser;

use bevy::picking::hover::Hovered;
use bevy::prelude::*;
use bevy::ui_widgets::{observe, Activate, Button};
use crate::obj_parser::parse_obj;

#[derive(Component)]
struct Position {
    x: f32,
    y: f32
}

fn print_position_system(query: Query<&Position>) {
    for position in &query {
        println!("position: {} {}", position.x, position.y);
    }
}

struct Entity(u64);

fn hello_world() {
    println!("hello world!");
}

#[derive(Component)]
struct Person;

#[derive(Component)]
struct Name(String);

fn add_people(mut commands: Commands) {
    commands.spawn((Person, Name("Elaina Proctor".to_string())));
    commands.spawn((Person, Name("Renzo Hume".to_string())));
    commands.spawn((Person, Name("Zayna Nieves".to_string())));
}

fn greet_people(query: Query<&Name, With<Person>>) {
    for name in &query {
        println!("hello {}!", name.0);
    }
}


fn setup_obj() {

    let obj = parse_obj("resources/Curve_1.obj");

    for face in obj.faces {
        println!("face: {:?}", face);
        for vertex in face {
            println!("vertex: {:?}", obj.vertices[vertex]);
        }
    }

}


fn main() {

    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, (setup, setup_obj))
        // .add_systems(Update, ( hello_world, greet_people))
        .run();
}

fn setup(mut commands: Commands, assets: Res<AssetServer>) {
    // ui camera
    commands.spawn(Camera2d);

    // A full-screen container that centers the button.
    commands.spawn((
        Node {
            width: percent(100),
            height: percent(100),
            align_items: AlignItems::Center,
            justify_content: JustifyContent::Center,
            ..default()
        },
        children![(
            button(&assets),
            // React to the button being clicked. `Button` fires an `Activate` event on a
            // completed click, so we attach an observer for it right here on the button entity.
            //
            // Try it: this is where you'd run your own logic (start a game, open a menu, etc.).
            observe(|_activate: On<Activate>| {
                info!("Button clicked!");
            }),
        )],
    ));
}

const NORMAL_BUTTON: Color = Color::srgb(0.15, 0.15, 0.15);
const HOVERED_BUTTON: Color = Color::srgb(0.25, 0.25, 0.25);
const PRESSED_BUTTON: Color = Color::srgb(0.35, 0.75, 0.35);

fn button(asset_server: &AssetServer) -> impl Bundle {
    (
        // The headless button widget. It handles pointer/keyboard input and pressed-state
        // tracking for us; we only provide the look below.
        Button,
        // `Hovered` is used by the picking backend to track if the pointer is over the button.
        Hovered::default(),
        Node {
            width: px(150),
            height: px(65),
            border: UiRect::all(px(5)),
            // horizontally center child text
            justify_content: JustifyContent::Center,
            // vertically center child text
            align_items: AlignItems::Center,
            border_radius: BorderRadius::MAX,
            ..default()
        },
        BorderColor::all(Color::BLACK),
        BackgroundColor(NORMAL_BUTTON),
        children![(
            Text::new("Button"),
            TextFont {
                font_size: FontSize::Px(33.0),
                ..default()
            },
            TextColor(Color::srgb(0.9, 0.9, 0.9)),
            TextShadow::default(),
        )],
    )
}